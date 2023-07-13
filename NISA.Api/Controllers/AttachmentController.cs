using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NISA.DataAccessLayer;
using NISA.Model;

namespace NISA.Api.Controllers
{
    
        [ApiController]
        [Route("api/[Controller]")]
        public class AttachmentController : Controller
        {
            public readonly DBContext dbContext;
            public AttachmentController(DBContext dbContext)
            {
                this.dbContext = dbContext;
            }
            [HttpGet]
            [Route("/AttachmentDetails")]
            public async Task<IActionResult> getAttachmentDetails()
            {
                return Ok(await dbContext.attachmentDetails.ToListAsync());
            }
            [HttpPost]
            [Route("/AttachmentDetails/{id:int}")]
            public async Task<IActionResult> addAttachmentDetails( InsertAttachmentRequest itr)
            {
                if (itr == null)
                {
                    return BadRequest("Enter valid details");
                }
                else
                {

                    AttachmentDetails attachmentDetails = new AttachmentDetails();                   
                    attachmentDetails.ticketId = this.dbContext.ticketDetails.Last().id;
                    attachmentDetails.fileName = itr.fileName;
                    attachmentDetails.isActive = true;
                    attachmentDetails.uploadedDate = itr.uploadedDate;
                    await dbContext.attachmentDetails.AddAsync(attachmentDetails);
                    await dbContext.SaveChangesAsync();
                    return Ok(attachmentDetails);
                }

            }
            

        }
    
}
