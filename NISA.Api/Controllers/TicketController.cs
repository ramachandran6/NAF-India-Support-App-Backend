using Azure;
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
            return Ok( await dbconn.ticketDetails.ToListAsync());
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
                int length = CountDigits(td.id + 1);
                string suffix = td.id.ToString().PadLeft(4 - length, '0');
                td.ticketRefnum = prefix + suffix + (td.id + 1);
                td.title = itr.title;
                td.description = itr.description;
                td.createdBy = dbconn.userDetails.FirstOrDefault(x => x.id == userId).name;
                td.departmentLookUpId = dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(itr.toDepartment)).id;
                td.endDate = itr.endDate;
                td.startDate = itr.startDate;
                int lookupid = (int)dbconn.userDetails.FirstOrDefault(x => x.id == userId).departmentLookupRefId;
                td.priotity = itr.priority;
                td.severity = itr.severity;
                td.attachments = itr.attachments;
                td.status = "assigned";
                List<UserDetails> tckHandlers = new List<UserDetails>(dbconn.userDetails.Where(x => x.departmentLookupRefId.Equals(td.departmentLookUpId)).AsQueryable());
                int numOfTickets = int.MaxValue;
                int user_id = 0;
                int prev_user_id = 0;
                tckHandlers.ForEach(x =>
                {
                    int num = dbconn.ticketDetails.Where(y => y.assignedTo == x.id).Count();
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

                await dbconn.ticketDetails.AddAsync(td);
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
        [Route("/TicketDetails/{ticketId:int}")]
        public async Task<IActionResult> UpdateTicketDetails([FromRoute] int ticketId, UpdateTicketDetailsRequest updateRequest)
        {
            if(updateRequest == null)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId);

                res.departmentLookUpId = string.IsNullOrEmpty(updateRequest.toDepartment) ? res.departmentLookUpId : dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(updateRequest.toDepartment)).id;
                res.endDate = string.IsNullOrEmpty(updateRequest.endDate)?res.endDate : updateRequest.endDate;
                res.priotity = updateRequest.priority == 0 ? res.priotity : updateRequest.priority;
                res.severity = updateRequest.severity == 0 ? res.severity : updateRequest.severity;
                res.attachments = string.IsNullOrEmpty(updateRequest.attachments) ? res.attachments : updateRequest.attachments;

                dbconn.ticketDetails.Update(res);
                dbconn.SaveChanges();

                return Ok(res);

            }
        }

        [HttpDelete]
        [Route("/TicketDetails/{ticketId:int}")]
        public async Task<IActionResult> DeleteTicketDetails([FromRoute] int ticketId)
        {
            //var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId).isDeleted = true;
            var res = dbconn.ticketDetails.FirstOrDefault(x=> x.id == ticketId);
            dbconn.ticketDetails.Remove(res);
            await dbconn.SaveChangesAsync();

            return Ok(res);

        }

        [HttpGet]
        [Route("/TicketDetails/{status}")]
        public async Task<IActionResult> GetByStatus([FromRoute] string status)
        {
            //int statusLookUpId = dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(status)).id;
            //var res = dbconn.ticketDetails.Where(x=> x.statusLookUpRefId == statusLookUpId).ToList();
            var res = dbconn.ticketDetails.Where(x=> x.status == status);
            return Ok(res);
        }

        [HttpGet]
        [Route("/TicketDetails/{startDate}&{endDate}")]
        public async Task<IActionResult> GetByDates([FromRoute] string startDate, [FromRoute] string endDate)
        {
            DateTime sd = DateTime.Parse(startDate);
            DateTime ed = DateTime.Parse(endDate);
            var res = dbconn.ticketDetails.Where(x => DateTime.Parse(x.startDate) >= sd && DateTime.Parse(x.startDate) <= ed);
            return Ok(res);
        }

        [HttpGet]
        [Route("/TicketDetails/{department}")]
        public async Task<IActionResult> GetById([FromRoute] string department)
        {
            var res = dbconn.ticketDetails.Where(x => x.departmentLookUpId == dbconn.lookUpTables.FirstOrDefault(x=> x.value.Equals(department)).id).ToList();
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
                List<UserDetails> tckHandlers = new List<UserDetails>(dbconn.userDetails.Where(x => x.departmentLookupRefId.Equals(res.departmentLookUpId)).AsQueryable());
                int numOfTickets = int.MaxValue;
                int user_id = 0;
                int prev_user_id = 0;
                tckHandlers.ForEach(x =>
                {
                    int num = dbconn.ticketDetails.Where(y => y.assignedTo == x.id).Count();
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
