using BeirutWalksDomains.Dto;
using BeirutWalksDomains.Models;
using BeirutWalksWebApi.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeirutWalksWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
            [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
            [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUpload )
        {
            ValidateFileUpload(imageUpload);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           var image = new Image()
           {
               File=imageUpload.File,
               FileDescription=imageUpload.Description,
               FileExtension=Path.GetExtension(imageUpload.File.FileName),
               FileSizeInBytes=imageUpload.File.Length,
               FileName=imageUpload.File.FileName,
           };
            await imageRepository.UploadImage(image);
            return Ok(image);
        }
        private void ValidateFileUpload(ImageUploadRequestDto image)
        {
            var imageExtension = new string[] { ".jpg", ".jpeg", ".png" };
            if(!imageExtension.Contains(Path.GetExtension(image.File.FileName).ToLower()))
            {
                ModelState.AddModelError("File", "Invalid file extension");
            }
            if (image.File.Length > 2 * 1024 * 1024)
            {
                ModelState.AddModelError("File", "File size should be less than 2MB");
            }
        }
    }
}
