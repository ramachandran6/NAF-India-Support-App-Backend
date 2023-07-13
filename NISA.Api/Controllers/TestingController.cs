using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestingController : ControllerBase
    {
        public readonly DBContext dbCon;
        public TestingController(DBContext dbCon)
        {
            this.dbCon = dbCon;
        }
        [HttpPost]
        [Route("/FileUpload")]
        public async Task<IActionResult> getFileUpload( InsertFileUpload itr)
        {
            if (itr == null)
            {
                return BadRequest("Enter valid details");
            }
            else
            {
                fileUpload file = new fileUpload();
                file.data = itr.data;   
                return Ok(file);

            }
        }



    }
}
