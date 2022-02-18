
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using FileUpload.Services.Interfaces;

namespace FileUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyFileUpload : ControllerBase
    {
        private IFileUploadService _fileUploadService { get; set; }
        public MyFileUpload(IFileUploadService fileUploadService)
        {
            _fileUploadService = fileUploadService;
        }

        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string jsonElement)
        {
            return Ok(await _fileUploadService.Upload(file,jsonElement));
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            return Ok(_fileUploadService.List());
        }

        [HttpDelete("Delete")]
        [Route("Delete/{name}")]
        public IActionResult Delete(string name)
        {
            return Ok(_fileUploadService.Delete(name));
        }


    }

}