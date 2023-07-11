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
        public async Task<IActionResult> getTicketDetails()
        {
            return Ok( await dbconn.ticketDetails.ToListAsync());
        }

        [HttpPost]
        [Route("/TicketDetails/{id:int}")]
        public async Task<IActionResult> addTicketDetails([FromRoute] int id,InsertTicketRequest itr)
        {
            if(itr == null)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                int count = dbconn.ticketDetails.Count();
                TicketDetails td = new TicketDetails();
                td.userId = id;
                //if (count == 0)
                //{
                //    td.id = 1;
                //}
                //else
                //{
                //    var res = dbconn.ticketDetails.Last();
                //    td.id = res.id+1;
                //}

                var prefix = "Tck";
                int length = countDigits(td.id+1);
                string suffix = td.id.ToString().PadLeft(4-length, '0');
                td.ticketRefnum = prefix+suffix+(td.id+1);
                td.title = itr.title;
                td.description = itr.description;
                td.createdBy = itr.createdBy;
                td.toDepartment = itr.toDepartment;
                td.endDate = itr.endDate;
                td.startDate = itr.startDate;
                td.userDepartment = itr.userDepartment;
                td.priotity = itr.priotity;
                td.severity = itr.severity;
                td.attachments = itr.attachments;
                td.status = "assigned";
                DateTime dt1 = DateTime.Parse(itr.startDate);
                DateTime dt2 = DateTime.Parse(itr.endDate);

                td.age = (int?)(dt2 - dt1).TotalDays;

                await dbconn.ticketDetails.AddAsync(td);
                await dbconn.SaveChangesAsync();
                return Ok(td);

            }

            
        }
        public static int countDigits(int number)
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
