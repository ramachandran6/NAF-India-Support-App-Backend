using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using NISA.Model;
using NISA.DataAccessLayer;



namespace NISA.Api.Controllers
{
    [ApiController]
    [Route("api/images")]

    public class ImageController : ControllerBase
    {

        public readonly DBContext _dbContext; 

        public ImageController(DBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            try
            {
                var file = Request.Form.Files[0]; 
                if (file == null || file.Length == 0)
                    return BadRequest("Invalid image file.");

                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    var image = new ImageEntity
                    {
                        FileName = file.FileName,
                        ContentType = file.ContentType,
                        ImageData = memoryStream.ToArray()
                    };

                    _dbContext.imageEntities.Add(image);
                    await _dbContext.SaveChangesAsync();
                }

                return Ok("Image uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading the image: " + ex.Message);
            }
        }

        [HttpGet("getImage/{imageId}")]
        public IActionResult GetImage(int imageId)
        {
            var image = _dbContext.imageEntities.FirstOrDefault(i => i.ImageId == imageId);
            if (image == null)
            {
                return NotFound();
            }

            return File(image.ImageData, image.ContentType);
        }

    }





}

