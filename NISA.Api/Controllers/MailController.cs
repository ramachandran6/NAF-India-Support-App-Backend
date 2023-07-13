using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using MimeKit;
using MailKit.Net.Smtp;

namespace NISA.Api.Controllers
{
    [Route("/MailController")]
    [ApiController]
    public class MailController : Controller
    {
        [HttpPost]
        public IActionResult SendEmail(String body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("ellanchikkumar@gmail.com"));
            email.To.Add(MailboxAddress.Parse("ellanchikkumar@gmail.com"));
            email.Subject="Test Email";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = body
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate("ellanchikkumar@gmail.com", "aqsptpnjckhgffsb");
            smtp.Send(email);
            smtp.Disconnect(true);
            return Ok();
        }


    }
}
