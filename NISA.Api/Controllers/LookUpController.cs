using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    [ApiController]
    
    public class LookUpController : Controller
    {
        public readonly DBContext dbconn;
        public LookUpController(DBContext dbconn)
        {
            this.dbconn = dbconn;
        }

        [HttpGet]
        [Route("LookupDetails")]
        public async Task<IActionResult> GetLookUpDetails()
        {
            return Ok( await dbconn.lookUpTables.ToListAsync());
        }

        [HttpGet]
        [Route("LookUpDetails/{id:int}")]
        public async Task<IActionResult> GetLookUpValue([FromRoute] int id)
        {
            return Ok(await dbconn.lookUpTables.FirstOrDefaultAsync(x => x.id == id));
        }

        [HttpPost]
        [Route("LookupDetails")]
        public async Task<IActionResult> AddLookUpDetails(InsertLookUpRequest ilr)
        {
            if(ilr == null)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                LookUpTable lut = new LookUpTable();
                lut.value = ilr.value;
                lut.category = ilr.category;

                await dbconn.lookUpTables.AddAsync(lut);
                await dbconn.SaveChangesAsync();

                return Ok(lut);
            }
        }


    }
}
