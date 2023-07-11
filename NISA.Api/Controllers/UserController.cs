using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        public readonly DBContext dbconn;

        public UserController(DBContext dbconn)
        {
            this.dbconn = dbconn;
        }

        [HttpGet]
        [Route("/User")]
        public async Task<IActionResult> GetUserDetails()
        {
            return Ok(await dbconn.userDetails.ToListAsync());
        }

        [HttpPost]
        [Route("/User")]
        public async Task<IActionResult> AddUserDetails(InsertUserDetailsRequest iur) 
        {
            if(iur == null)
            {
                return BadRequest("enter valid details");
            }
            else
            {
                UserDetails ud = new UserDetails();

                var prefix = "NAF";
                int length = countDigits(ud.id + 1);
                string suffix = ud.id.ToString().PadLeft(4 - length, '0');
                ud.userName = prefix + suffix + (ud.id + 1);
                ud.name = iur.name;
                ud.email = iur.email;
                ud.password = iur.password;
                ud.department = iur.department;
                ud.isActive = true;
                ud.phoneNumber = iur.phoneNumber;

                await dbconn.userDetails.AddAsync(ud);
                await dbconn.SaveChangesAsync();

                return Ok(ud);
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
