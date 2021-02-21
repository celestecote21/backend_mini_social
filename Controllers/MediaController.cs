using System.Web;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using netart.Models;
using netart.JWT;
using netart.Services;
using netart.Utilities;
using netart.LogicalCore;

namespace netart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly GoogleStorageService _storageService;
        private readonly PostCore _postCore;

        public MediaController(GoogleStorageService storage, PostCore postCore) {
            _storageService = storage;
            _postCore = postCore;
        }

        [HttpPost]
        [Route("upload/{uuid}")]
        [Authorize]
        public async Task<ActionResult<dynamic>> ReceiveMediaPost(string uuid)
        {
            var file = Request.Form.Files[0];
            var fileType = file.ContentType;
            await _storageService.UploadFileGoogle(file, uuid, fileType);
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            // todo: check the right to upload
            return username;
        }

        [HttpGet]
        [Route("download/")]
        [Authorize]
        public async Task<FileContentResult> DownloadMediaPost([FromQuery] Post post)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var context = HttpContext.Response.Headers;
            var memoryStream = await _storageService.DownloadFileGoogle(post.FileUuid);
            return new FileContentResult(memoryStream.ToArray(), "application/stream-octet");
            //return File(memoryStream, "application/stream-octet");
        }
    }
}