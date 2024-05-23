using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SSLAudit.Controllers.API
{
    [Route("api/uploadimage")] //E:\Sage\SSLAudit\SageERP\Controllers\API\ImageUploadController.cs
    [ApiController]
    public class ImageUploadController : ControllerBase
    {

        [HttpPost]
        public IActionResult UploadImage(IFormFile file)
        {
            return Ok(new { imageUrl = "/path/to/uploaded/image.jpg" });
        }


    }
}
