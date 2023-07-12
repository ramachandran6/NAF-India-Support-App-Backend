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
        [Route("/TicketDetails/{id:int}")]
        public async Task<IActionResult> AddTicketDetails([FromRoute] int id,InsertTicketRequest itr)
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
                var refnum = 1;
                if(count != 0)
                {
                    var res = (from tickets in dbconn.ticketDetails orderby tickets.id descending select tickets).FirstOrDefault(); // sort by descending order based on id and selects the first row
                    refnum = res.id + 1;
                }
                
               
                var prefix = "Tck";
                int length = CountDigits(refnum);
                string suffix = td.id.ToString().PadLeft(4-length, '0');
                td.ticketRefnum = prefix+suffix+refnum;
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
                List<UserDetails> tckHandlers = new List<UserDetails>(dbconn.userDetails.Where(x => x.department.Equals(itr.toDepartment)).AsQueryable());
                int numOfTickets = int.MaxValue;
                int user_id = 0;
                tckHandlers.ForEach(x =>
                {
                    var result = from tick in dbconn.ticketHandlingDetails where tick.deptUserId.Equals(x.id) select tick;
                    if (result.Count() < numOfTickets)
                    {
                        numOfTickets = result.Count();
                        user_id = (int)x.id;
                    }
                });
                td.owner = dbconn.userDetails.FirstOrDefault(x=> x.id == user_id).name;
                DateTime date = DateTime.Today;
                DateTime dt2 = DateTime.Parse(itr.endDate);

                td.age = (int?)(dt2 - date).TotalDays;

                await dbconn.ticketDetails.AddAsync(td);
                await dbconn.SaveChangesAsync();
                return Ok(td);

            }

            
        }

        [HttpGet]
        [Route("/TicketDetails/{status}")]
        public async Task<IActionResult> GetByStatus([FromRoute] string status)
        {
            var res = dbconn.ticketDetails.Where(x=> x.status == status).ToList();
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
        [Route("/TicketDetails/{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var res = dbconn.ticketDetails.Where(x => x.id == id).ToList();
            return Ok(res);
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
