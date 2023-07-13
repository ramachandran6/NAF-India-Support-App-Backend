using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;
using System;

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
            
            if (iur == null)
            {
                return BadRequest("enter valid details");
            }
            else
            {
                int count = dbconn.userDetails.Count();
                var suffixnum = 1;
                if (count != 0)
                {
                    var res = (from user in dbconn.userDetails orderby user.id descending select user).FirstOrDefault(); // sort by descending order based on id and selects the first row
                    suffixnum = res.id + 1;
                }
                UserDetails ud = new UserDetails();

                var prefix = "NAF";
                int length = CountDigits(suffixnum);
                string suffix = ud.id.ToString().PadLeft(4 - length, '0');
                ud.userName = prefix + suffix + suffixnum;
                ud.name = iur.name;
                ud.email = iur.email;
                ud.password = iur.password; //encodePassword(iur.password);
                ud.lookupRefId = dbconn.lookUpTables.FirstOrDefault(x=> x.value.Equals(iur.department)).id;
                ud.isActive = true;
                ud.phoneNumber = iur.phoneNumber;

                await dbconn.userDetails.AddAsync(ud);
                await dbconn.SaveChangesAsync();

                return Ok(ud);
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
        //public string encodePassword(string password)
        //{
        //    byte[] encData_byte = new byte[password.Length];
        //    encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
        //    string encodedData = Convert.ToBase64String(encData_byte);
        //    return encodedData;

        //    //decode
        //    //System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
        //    //System.Text.Decoder utf8Decode = encoder.GetDecoder();
        //    //byte[] todecode_byte = Convert.FromBase64String(encodedData);
        //    //int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
        //    //char[] decoded_char = new char[charCount];
        //    //utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
        //    //string result = new String(decoded_char);
        //    //return result;
        //}


    }
}
