using Microsoft.AspNetCore.Mvc;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TicketCommentController : Controller
    {
        public readonly DBContext dbconn;

        public TicketCommentController(DBContext dbconn)
        {
            this.dbconn = dbconn;
        }

        [HttpGet]
        [Route("TicketComment/{ticketRefnum}")]
        public async Task<IActionResult> GetTicketComments([FromRoute] string ticketRefnum)
        {
            if (string.IsNullOrEmpty(ticketRefnum))
            {
                return BadRequest("Enter ticketrefnumber");
            }
            
            var res = from ticketComment in dbconn.ticketComments where ticketComment.ticketRefnum == ticketRefnum orderby ticketComment.commentedOn select ticketComment;
            if(res == null)
            {
                return NotFound("Ticket reference number not found");
            }
            return Ok(res);
        }

        [HttpPost]
        [Route("TicketComment/{userId:int}")]
        public async Task<IActionResult> AddTicketComment([FromRoute] int userId,InsertCommentRequest icr)
        {
            if(icr == null)
            {
                return BadRequest("Enter some valid details");
            }
            if (dbconn.userDetails.FirstOrDefault(x => x.id == userId) == null)
            {
                return NotFound("userId not found");
            }
            var name = dbconn.userDetails.FirstOrDefault(x => x.id == userId).name;
            TicketComments tickCom = new TicketComments();
            tickCom.comment = icr.comment;
            tickCom.ticketRefnum = icr.ticketRefnum;
            tickCom.commentedBy = name;
            tickCom.commentedOn = DateTime.Now;

            await dbconn.ticketComments.AddAsync(tickCom);
            await dbconn.SaveChangesAsync();

            return Ok(tickCom);
        }
    }
}
