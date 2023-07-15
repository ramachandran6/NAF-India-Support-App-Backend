using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;

namespace NISA.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class TicketHistoryController : Controller
    {
        public readonly DBContext dbconn;

        public TicketHistoryController(DBContext dbconn)
        {
            this.dbconn = dbconn;
        }

        [HttpGet]
        [Route("/TicketHistory/{ticketRefNum}")]
        public async Task<IActionResult> GetTicketsHistory([FromRoute] string ticketRefNum)
        {
            return Ok( await dbconn.ticketHistoryTables.Where(x=> x.ticketRefNum.Equals(ticketRefNum)).ToListAsync());
        }

        //[HttpGet]
        //[Route("/TicketHistory/{ticketRefNum:string}")]
        //public async Task<IActionResult> GetTicketHistory([FromRoute] string ticketRefNum)
        //{
        //    //var res = await dbconn.ticketHistoryTables.Where(a=> a.ticketRefNum == ticketRefNum).GroupBy(a=> a.ticketRefNum).OrderBy(a=> a.Min(n=> n.updatedOn)).ToListAsync();
        //    //var res = from ticket in dbconn.ticketHistoryTables where ticket.ticketRefNum == ticketRefNum orderby ticket.updatedOn select ticket;
        //    var res = await dbconn.ticketHistoryTables.Where(x => x.ticketRefNum.Equals(ticketRefNum)).ToListAsync();
        //    return Ok(res);
        //    //dbconn.ticketHistoryTables.Where(x => x.ticketRefNum.Equals(ticketRefNum))
        //}
    }
}
