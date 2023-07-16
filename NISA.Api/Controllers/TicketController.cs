﻿using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TicketController : Controller
    {
        public readonly DBContext dbconn;

        public TicketController(DBContext dbconn)
        {
            this.dbconn = dbconn;
        }

        [HttpGet]
        [Route("/TicketDetails")]
        public async Task<IActionResult> GetTicketDetails()
        {
            return Ok( await dbconn.ticketDetails.Where(x=> x.isDeleted == false).ToListAsync());
        }

        [HttpGet]
        [Route("/TicketDetailsByRef/{ticketRefnum}")]
        public async Task<IActionResult> GetTicketDetailsById([FromRoute] string ticketRefnum)
        {
            return Ok(dbconn.ticketDetails.Where(x => x.ticketRefnum.Equals(ticketRefnum) && x.isDeleted == false).ToList());
        }

        [HttpPost]
        [Route("/TicketDetails/{userId:int}")]
        public async Task<IActionResult> AddTicketDetails([FromRoute] int userId, InsertTicketRequest itr)

        {
            if (itr == null)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                int count = dbconn.ticketDetails.Count();
                TicketDetails td = new TicketDetails();
                td.userId = userId;
                var refnum = 1;
                if (count != 0)
                {
                    var res = (from tickets in dbconn.ticketDetails orderby tickets.id descending select tickets).FirstOrDefault(); // sort by descending order based on id and selects the first row
                    refnum = res.id + 1;
                }
                var prefix = "Tck";
                int length = CountDigits(refnum);
                string suffix = td.id.ToString().PadLeft(4 - length, '0');
                td.ticketRefnum = prefix + suffix + (refnum);
                td.title = itr.title;
                td.description = itr.description;
                td.createdBy = dbconn.userDetails.FirstOrDefault(x => x.id == userId).name;
                td.departmentLookUpId = dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(itr.toDepartment)).id;
                td.endDate = itr.endDate;
                td.startDate = itr.startDate;
                int lookupid = (int)dbconn.userDetails.FirstOrDefault(x => x.id == userId).departmentLookupRefId;
                td.priority = itr.priority;
                td.severity = itr.severity;
                td.attachments = itr.attachments;
                td.status = "assigned";
                List<UserDetails> tckHandlers = new List<UserDetails>(dbconn.userDetails.Where(x => x.departmentLookupRefId.Equals(td.departmentLookUpId) && x.isActive == true).AsQueryable());
                int numOfTickets = int.MaxValue;
                int user_id = 0;
                int prev_user_id = 0;
                tckHandlers.ForEach(x =>
                {
                    int num = dbconn.ticketDetails.Where(y => y.assignedTo == x.id && y.isDeleted == false).Count();
                    if (numOfTickets > num)
                    {
                        numOfTickets = num;
                        prev_user_id = user_id;
                        user_id = x.id;
                    }
                });
                if (user_id == userId)
                {
                    td.assignedTo = prev_user_id;
                }
                else
                {
                    td.assignedTo = user_id;
                }
                td.owner = dbconn.userDetails.FirstOrDefault(x => x.id == td.assignedTo).name;
                DateTime date = DateTime.Today;
                DateTime dt2 = DateTime.Parse(itr.endDate);

                td.age = (int?)(dt2 - date).TotalDays;
                td.isDeleted = false;
                td.isReopened = false;

                await dbconn.ticketDetails.AddAsync(td);
                await dbconn.SaveChangesAsync();

                TicketHistoryTable ticketHistory = new TicketHistoryTable();
                ticketHistory.ticketRefNum = td.ticketRefnum;
                ticketHistory.status = td.status;
                ticketHistory.priority = td.priority;
                ticketHistory.severity = td.severity;
                ticketHistory.departmentLookUpRefId = td.departmentLookUpId;
                ticketHistory.attachments = td.attachments;
                ticketHistory.endDate = td.endDate;
                ticketHistory.updatedBy = userId;
                ticketHistory.updatedOn = DateTime.Now;
                
                await dbconn.ticketHistoryTables.AddAsync(ticketHistory);
                await dbconn.SaveChangesAsync();

                return Ok(td);

            }
        }


        //[HttpPatch]
        //[Route("/TicketDetails/{ticketId:int}")]
        //public async Task<IActionResult> UpdateTicketDetails([FromRoute] int ticketId,[FromBody] JsonPatchDocument<UpdateTicketDetailsRequest> utr)
        //{
        //    try
        //    {
        //        var res = dbconn.ticketDetails.FirstOrDefault(x => x.id == ticketId);
        //        if(res == null)
        //        {
        //            return BadRequest("Enter Valid details");
        //        }
        //        else
        //        {
        //            utr.ApplyTo(res, ModelState);
        //        }
        //    }
        //}

        [HttpPut]
        [Route("/TicketDetails/{ticketId:int}&{userId:int}")]
        public async Task<IActionResult> UpdateTicketDetails([FromRoute] int ticketId, [FromRoute] int userId, UpdateTicketDetailsRequest updateRequest)
        {
            if(updateRequest == null || dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId).isDeleted == true)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId);

                res.departmentLookUpId = string.IsNullOrEmpty(updateRequest.toDepartment) ? res.departmentLookUpId : dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(updateRequest.toDepartment)).id;
                res.endDate = string.IsNullOrEmpty(updateRequest.endDate)?res.endDate : updateRequest.endDate;
                DateTime date = DateTime.Today;
                DateTime dt2 = DateTime.Parse(res.endDate);
                res.age = (int?)(dt2 - date).TotalDays;

                res.priority = updateRequest.priority == 0 ? res.priority : updateRequest.priority;
                res.severity = updateRequest.severity == 0 ? res.severity : updateRequest.severity;
                res.attachments = string.IsNullOrEmpty(updateRequest.attachments) ? res.attachments : updateRequest.attachments;
                res.status = string.IsNullOrEmpty(updateRequest.status) ? res.status : updateRequest.status;

                if(res.status == "completed")
                {
                    res.endDate = DateTime.Now.ToString();
                    res.age = 0;

                }

                dbconn.ticketDetails.Update(res);
                dbconn.SaveChanges();

                TicketHistoryTable ticketHistory = new TicketHistoryTable();
                ticketHistory.ticketRefNum = res.ticketRefnum;
                ticketHistory.status = res.status;
                ticketHistory.priority = res.priority;
                ticketHistory.severity = res.severity;
                ticketHistory.departmentLookUpRefId = res.departmentLookUpId;
                ticketHistory.attachments = res.attachments;
                ticketHistory.endDate = DateTime.Now.ToString();
                ticketHistory.updatedBy = userId;
                ticketHistory.updatedOn = DateTime.Now;

                await dbconn.ticketHistoryTables.AddAsync(ticketHistory);
                await dbconn.SaveChangesAsync();

                return Ok(res);

            }
        }

        [HttpPut]
        [Route("/reopenTicket/{ticketId:int}")]
        public async Task<IActionResult> ReopenTicket([FromRoute] int ticketId,ReopenTicketRequest rtr)
        {
            if(rtr == null)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                
                var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId);
                if(res.age != 0)
                {
                    return BadRequest("You cant reopen ticket now");
                }
                int previousTicketHandlerId = res.assignedTo;

                List<UserDetails> tckHandlers = new List<UserDetails>(dbconn.userDetails.Where(x => x.departmentLookupRefId.Equals(res.departmentLookUpId) && x.isActive == true).AsQueryable());
                int numOfTickets = int.MaxValue;
                int user_id = 0;
                int prev_user_id = 0;
                tckHandlers.ForEach(x =>
                {
                    int num = dbconn.ticketDetails.Where(y => y.assignedTo == x.id && y.isDeleted == false).Count();
                    if (numOfTickets > num && x.departmentLookupRefId != previousTicketHandlerId)
                    {
                        numOfTickets = num;
                        prev_user_id = user_id;
                        user_id = x.id;
                    }
                });
                if (user_id == res.id)
                {
                    res.assignedTo = prev_user_id;
                }
                else
                {
                    res.assignedTo = user_id;
                }
                res.startDate = DateTime.Now.ToString();
                res.endDate = rtr.endDate;
                res.owner = dbconn.userDetails.FirstOrDefault(x => x.id == res.assignedTo).name;
                res.status = "assigned";
                res.isReopened = true;

                dbconn.ticketDetails.Update(res);
                await dbconn.SaveChangesAsync();
                return Ok(res);

            }
        }

        [HttpDelete]
        [Route("/TicketDetails/{ticketId:int}&{userId:int}")]
        public async Task<IActionResult> DeleteTicketDetails([FromRoute] int ticketId, [FromRoute] int userId)
        {
            //var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId).isDeleted = true;
            var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId);
            res.status = "deleted";
            res.isDeleted = true;
            dbconn.ticketDetails.Update(res);
            await dbconn.SaveChangesAsync();

            TicketHistoryTable ticketHistory = new TicketHistoryTable();
            ticketHistory.ticketRefNum = res.ticketRefnum;
            ticketHistory.status = "deleted";
            ticketHistory.priority = res.priority;
            ticketHistory.severity = res.severity;
            ticketHistory.departmentLookUpRefId = res.departmentLookUpId;
            ticketHistory.attachments = res.attachments;
            ticketHistory.endDate = res.endDate;
            ticketHistory.updatedBy = userId;
            ticketHistory.updatedOn = DateTime.Now;

            await dbconn.ticketHistoryTables.AddAsync(ticketHistory);
            await dbconn.SaveChangesAsync();

            return Ok(res);

        }

        [HttpGet]
        [Route("/TicketDetailsByStatus/{status}")]
        public async Task<IActionResult> GetByStatus([FromRoute] string status)
        {
            if (status.Equals("all"))
            {
                return Ok(dbconn.ticketDetails.Where(x=> x.isDeleted == false).ToList());
            }
            //int statusLookUpId = dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(status)).id;
            //var res = dbconn.ticketDetails.Where(x=> x.statusLookUpRefId == statusLookUpId).ToList();
            var res = dbconn.ticketDetails.Where(x=> x.status == status && x.isDeleted == false);
            return Ok(res);
        }

        //[HttpGet]
        //[Route("/TicketDetails/{startDate}&{endDate}")]
        //public async Task<IActionResult> GetByDates([FromRoute] string startDate, [FromRoute] string endDate)
        //{
        //    DateTime sd = DateTime.Parse(startDate);
        //    DateTime ed = DateTime.Parse(endDate);
        //    var res = dbconn.ticketDetails.Where(x => DateTime.Parse(x.startDate) >= sd && DateTime.Parse(x.startDate) <= ed );
        //    return Ok(res);
        //}

        [HttpGet]
        [Route("/TicketDetails/{department}")]
        public async Task<IActionResult> GetById([FromRoute] string department)
        {
            var res = dbconn.ticketDetails.Where(x => x.departmentLookUpId == dbconn.lookUpTables.FirstOrDefault(x=> x.value.Equals(department)).id && x.isDeleted == false).ToList();
            return Ok(res);
        }

        [HttpGet]
        [Route("/Escalate/{ticketId:int}")]
        public async Task<IActionResult> EscalateTicket([FromRoute] int ticketId)
        {
            var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId);
            if(res == null)
            {
                return BadRequest("TicketId not found");
            }
            else
            {
                int previousTicketHandlerId = res.assignedTo;

                List<UserDetails> tckHandlers = new List<UserDetails>(dbconn.userDetails.Where(x => x.departmentLookupRefId.Equals(res.departmentLookUpId) && x.isActive == true).AsQueryable());
                int numOfTickets = int.MaxValue;
                int user_id = 0;
                int prev_user_id = 0;
                tckHandlers.ForEach(x =>
                {
                    int num = dbconn.ticketDetails.Where(y => y.assignedTo == x.id && y.isDeleted == false).Count();
                    if (numOfTickets > num && x.departmentLookupRefId != previousTicketHandlerId)
                    {
                        numOfTickets = num;
                        prev_user_id = user_id;
                        user_id = x.id;
                    }
                });
                if (user_id == res.id)
                {
                    res.assignedTo = prev_user_id;
                }
                else
                {
                    res.assignedTo = user_id;
                }
                res.owner = dbconn.userDetails.FirstOrDefault(x => x.id == res.assignedTo).name;

                dbconn.ticketDetails.Update(res);
                await dbconn.SaveChangesAsync();

                return Ok(res);
            }
        }
        public static int CountDigits(int number)
        {
            int count = 0;
            while (number > 0)
            {
                number = number / 10;
                count++;
            }
            return count;
        }

    }
}
