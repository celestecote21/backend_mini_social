using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PostController : ControllerBase
    {

        private readonly PostCore _postCore;
        public PostController(PostCore postCore)
        {
            _postCore = postCore;
        }

        [HttpGet]
        [Route("fake")]
        [AllowAnonymous]
        public ActionResult<dynamic> FakePost()
        {
            return (new[] {
                        new {
                            bdId = "string",
                            username = "string",
                            title = "string",
                            content = "string",
                            creationDate = "2021-02-20T22:31:56.117Z"
                        },
                        new {
                            bdId = "string",
                            username = "string",
                            title = "string",
                            content = "string",
                            creationDate = "2021-02-20T22:31:56.117Z"
                        }
                    });
        }

        [HttpPost]
        [Route("newPost")]
        [Authorize]
        public ActionResult<dynamic> CreatePost([FromBody] Post post)
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            if (username != post.Username)
                return BadRequest(new { message = "your user can't post here" });
            _postCore.CreateNew(post);
            return post;
        }

        [HttpGet]
        [Route("subscribePost")]
        [Authorize]
        public ActionResult<dynamic> GetSubscribePost()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var posts = _postCore.GetSubscribePost(username);
            if (posts == null)
                return (new { message = "no post" });
            return posts;
        }

        [HttpGet]
        [Route("top")]
        [Authorize]
        public ActionResult<dynamic> GetTopPost()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var posts = _postCore.GetSubscribePost(username);
            if (posts == null)
                return (new { message = "no post" });
            return posts;
        }

        [HttpGet]
        [Route("myPost")]
        [Authorize]
        public ActionResult<dynamic> GetMyPost()
        {
            var username = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            return _postCore.GetMyPost(username);
        }
    }
}
