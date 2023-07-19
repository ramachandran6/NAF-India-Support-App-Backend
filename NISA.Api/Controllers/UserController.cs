using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using NISA.DataAccessLayer;
using NISA.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static System.Net.WebRequestMethods;

namespace NISA.Api.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : Controller
    {
        public readonly DBContext dbconn;
        public readonly IConfiguration configuration;

        public UserController(DBContext dbconn, IConfiguration configuration)
        {
            this.dbconn = dbconn;
            this.configuration = configuration;
        }

        private string createtoken(UserDetails user)
        {
            var a = user.email;
            List<Claim> claims = new List<Claim> {
                    new Claim(ClaimTypes.Name,user.email),
                   new Claim(ClaimTypes.Role,dbconn.userDetails.Find(user.id).department)
                };
            var key = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(
                configuration.GetSection("Appsettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        [HttpGet]
        [Route("/User")]
        public async Task<IActionResult> GetUserDetails()
        {
            return Ok(await dbconn.userDetails.Where(x => x.isActive == true).ToListAsync());
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
                ud.departmentLookupRefId = dbconn.lookUpTables.FirstOrDefault(x => x.value.Equals(iur.department)).id;
                ud.isActive = true;
                ud.isLoggedIn = false;
                ud.phoneNumber = iur.phoneNumber;

                if (dbconn.userDetails.FirstOrDefault(x => x.email.Equals(ud.email)) != null)
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
                    Text = " Hi " + ud.name + "  <br> " + "Your account has been created <br>" + "Your username is :" + ud.userName + "<br>Your temporary password is :" + ud.password + "<br> <br> <br> Naf India Support team "
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
        [Authorize (Roles ="admin")]
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
                var a = createtoken(res);
                JwtModal jwtModal= new JwtModal();
                jwtModal.email = res.email;
                jwtModal.id = res.id;
                jwtModal.name = res.name;
                jwtModal.Jwt = a;
                jwtModal.department = res.department;
               
                return Ok(jwtModal);

            }
        }
        //Forget Password otp sending
        [HttpGet]
        [Route("/forgetPassword/{userEmail}")]
        public async Task<IActionResult> ForgetPassword([FromRoute] string userEmail)
        {
            var userDetails = await dbconn.userDetails.FirstOrDefaultAsync(x=> x.email.Equals(userEmail));
            if (userDetails == null)
            {
                return BadRequest("entered email does not exist");
            }
            else
            {
                Random rnd = new Random();
                var otp = rnd.Next(1111, 9999);

                //Send otp through mail;
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("ellanchikkumar@gmail.com"));
                email.To.Add(MailboxAddress.Parse(userDetails.email));
                email.Subject = "Sending verification code for your changing your account password";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = " Hi " + userDetails.name + "  <br> " + "Otp generated for changing your password is : " + otp + "<br>Don't share this to anyone <br> <br> <br> Naf India Support team "
                };
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("ellanchikkumar@gmail.com", "aqsptpnjckhgffsb");
                smtp.Send(email);
                smtp.Disconnect(true);


                return Ok(otp);

            }
            
            
        }

        //change password using otp
        [HttpPut]
        [Route("/changePasswordUsingOtp/{userEmail}")]
        public async Task<IActionResult> UpdatePasswordUsingOtp([FromRoute] string userEmail, ChangePasswordUsingOtp cp)
        {
            if (cp.newPassword == null)
            {
                return BadRequest("enter valid password");
            }else if( userEmail == null)
            {
                return BadRequest("enter valid email");
            }else
            {
                var res = dbconn.userDetails.FirstOrDefault(x => x.email == userEmail);
                res.password = cp.newPassword;             
                dbconn.userDetails.Update(res);
                await dbconn.SaveChangesAsync();

                //Send password change confirmation mail
                var email = new MimeMessage();
                email.From.Add(MailboxAddress.Parse("ellanchikkumar@gmail.com"));
                email.To.Add(MailboxAddress.Parse(res.email));
                email.Subject = "Sending password change confirmation mail";
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = " Hi " + res.name + "  <br> Your password is successfully changed <br> <br> <br> Naf India Support team "
                };
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                smtp.Authenticate("ellanchikkumar@gmail.com", "aqsptpnjckhgffsb");
                smtp.Send(email);
                smtp.Disconnect(true);

                return Ok(res);
            }
        }

        // Change password from existing password
        [HttpPut]
        [Route("/changePasswordUsingExistingPassword/{userEmail}")]
        public async Task<IActionResult> UpdatePasswordUsingExistingPassword([FromRoute] string userEmail,UpdatePasswordFromExistingPassword passwordDetails)
        {
            if (passwordDetails.newPassword == null)
            {
                return BadRequest("enter valid password");
            }
            else if (userEmail == null)
            {
                return BadRequest("enter valid email");
            }
            else
            {
                var res = dbconn.userDetails.FirstOrDefault(x => x.email == userEmail);
                if(res.password != passwordDetails.oldPassword)
                {
                    return BadRequest("Old password is wrong");
                }else if(passwordDetails.newPassword != passwordDetails.confirmNewPassword)
                {
                    return BadRequest("entered new password and confirm new password does not match");
                }
                else
                {
                    res.password = passwordDetails.newPassword;
                    dbconn.userDetails.Update(res);
                    await dbconn.SaveChangesAsync();

                    //Send password change confirmation mail
                    var email = new MimeMessage();
                    email.From.Add(MailboxAddress.Parse("ellanchikkumar@gmail.com"));
                    email.To.Add(MailboxAddress.Parse(res.email));
                    email.Subject = "Sending password change confirmation mail";
                    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = " Hi " + res.name + "  <br> Your password is successfully changed <br> <br> <br> Naf India Support team "
                    };
                    using var smtp = new MailKit.Net.Smtp.SmtpClient();
                    smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    smtp.Authenticate("ellanchikkumar@gmail.com", "aqsptpnjckhgffsb");
                    smtp.Send(email);
                    smtp.Disconnect(true);

                    return Ok(res);


                }


                
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
