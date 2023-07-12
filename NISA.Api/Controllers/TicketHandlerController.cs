using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    [ApiController]
    public class TicketHandlerController : Controller
    {
        public readonly DBContext dbConn;
        public TicketHandlerController(DBContext dbConn)
        {
            this.dbConn = dbConn;
        }

        [HttpGet]
        [Route("TicketHandling")]
        public async Task<IActionResult> GetTicketHandlingDetails()
        {
            return Ok(await dbConn.ticketHandlingDetails.ToListAsync());
        }

        [HttpPost]
        [Route("TicketHandling/{ticketRefNumber}")]
        public async Task<IActionResult> AssignHandlingDetails([FromRoute] string ticketRefNumber)
        {
            if (string.IsNullOrEmpty(ticketRefNumber))
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                var res = dbConn.ticketHandlingDetails.FirstOrDefault(x => x.ticketId.Equals(ticketRefNumber));
                if (res != null)
                {
                    return BadRequest("Ticket already assigned");
                }
                var department = dbConn.ticketDetails.FirstOrDefault(x => x.ticketRefnum.Equals(ticketRefNumber)).toDepartment;
                List<UserDetails> tckHandlers = new List<UserDetails>(dbConn.userDetails.Where(x => x.department.Equals(department)).AsQueryable());
                int numOfTickets = int.MaxValue;
                int id = 0;
                tckHandlers.ForEach(x =>
                {
                    var result = from tick in dbConn.ticketHandlingDetails where tick.deptUserId.Equals(x.id) select tick;
                    if (result.Count() < numOfTickets)
                    {
                        numOfTickets = result.Count();
                        id = (int)x.id;
                    }
                });

                TicketHandlingDetails thd = new TicketHandlingDetails();
                thd.genUserId = dbConn.ticketDetails.FirstOrDefault(x => x.ticketRefnum.Equals(ticketRefNumber)).userId;
                thd.deptUserId = id;
                thd.ticketId = ticketRefNumber;
                thd.ticketHandleId = new Guid();
                dbConn.ticketDetails.FirstOrDefault(x => x.ticketRefnum.Equals(ticketRefNumber)).owner = dbConn.userDetails.FirstOrDefault(x=> x.id == id).name;
                await dbConn.ticketHandlingDetails.AddAsync(thd);
                await dbConn.SaveChangesAsync();

                return Ok(thd);

            }

        }

        
        
    }
}
