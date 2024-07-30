using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectWebApiCore.Interface;
using System.Net;
using System.Net.Http.Headers;

namespace ProjectWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UploadController : Controller
    {
        private readonly ILogger<UploadController> _logger;

        public UploadController(ILogger<UploadController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        [HttpPost]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> UploadImage(string? siteId)
        {
            try
            {
                _logger.LogInformation("开始上传文件");
                if (Request.Form.Files.Count == 0)
                {
                    return BadRequest("没有文件上传");
                }
                var file = Request.Form.Files[0];
                var folderName = Path.Combine("Images", siteId);
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);
                }
                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var fullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);                       
                        stream.Flush();
                    }

                    return Ok(new { FilePath = dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "上传文件时发生错误");
                return StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
