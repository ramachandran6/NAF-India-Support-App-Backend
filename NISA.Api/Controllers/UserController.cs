using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit;
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
            return Ok(await dbconn.userDetails.Where(x=> x.isActive == true).ToListAsync());
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
                    suffixnum = (int)(res.id + 1);
                }
                UserDetails ud = new UserDetails();

                var prefix = "NAF";
                int length = CountDigits(suffixnum);
                string suffix = ud.id.ToString().PadLeft(4 - length, '0');
                ud.userName = prefix + suffix + suffixnum;
                ud.name = iur.name;
                ud.email = iur.email;
                ud.password = iur.password; //encodePassword(iur.password);
                ud.department = iur.department;
                ud.departmentLookupRefId = dbconn.lookUpTables.FirstOrDefault(x=> x.value.Equals(iur.department)).id;
                ud.isActive = true;
                ud.isLoggedIn = false;
                ud.phoneNumber = iur.phoneNumber;

                if(dbconn.userDetails.FirstOrDefault(x=> x.email.Equals(ud.email)) != null)
                {
                    return BadRequest("email id already exists");
                }

                await dbconn.userDetails.AddAsync(ud);
                await dbconn.SaveChangesAsync();

                //SendEmail(ud);
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("ellanchikkumar@gmail.com"));
                email.To.Add(MailboxAddress.Parse(ud.email));
                email.Subject = "Confirmation mail for account creation";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = " Hi " + ud.name + "  <br> " + "Your account has been created <br>" + "Your username is :" + ud.userName + "<br>Your temporary password is :" + ud.password
                };
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("ellanchikkumar@gmail.com", "aqsptpnjckhgffsb");
                smtp.Send(email);
                smtp.Disconnect(true);


                return Ok(ud);
            }
        }

        [HttpDelete]
        [Route("/User/{userId:int}")]
        public async Task<IActionResult> DeleteUser([FromRoute] int userId)
        {
            var res = await dbconn.userDetails.FirstOrDefaultAsync(x => x.id == userId);
            if(res == null)
            {
                return BadRequest("Id not found");
            }
            else
            {
                res.isActive = false;
                
                dbconn.userDetails.Update(res);
                await dbconn.SaveChangesAsync();

                return Ok(res);
            }
        }

        [HttpGet]
        [Route("/Login/{email}&{password}")]
        public async Task<IActionResult> UserLogin([FromRoute] string email, [FromRoute] string password)
        {
            if (email == null || password == null)
            {
                return BadRequest("Username or password is not filled");
            }
            else
            {
                var res = dbconn.userDetails.FirstOrDefault(x => x.email.Equals(email));
                if (res == null)
                {
                    return BadRequest("Email id not found");
                }
                string resPassword = res.password;
                if (!res.password.Equals(password))
                {
                    return BadRequest("Invalid password");
                }
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

        [HttpPut]
        [Route("/User/{userId:int}")]
        public async Task<IActionResult> UpdateUserDetails([FromRoute] int userId,UpdateUserDetails updateRequest)
        {
            if(updateRequest == null)
            {
                return BadRequest("enter valid details");
            }
            else
            {
                var res = dbconn.userDetails.FirstOrDefault(x=> x.id  == userId);
                res.name = string.IsNullOrEmpty(updateRequest.name) ? res.name : updateRequest.name;
                res.phoneNumber = string.IsNullOrEmpty(updateRequest.phoneNumber) ? res.phoneNumber : updateRequest.phoneNumber;

                dbconn.userDetails.Update(res);
                await dbconn.SaveChangesAsync();

                return Ok(res);
            }
        }

        
    }
}
