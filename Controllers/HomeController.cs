using System;
using System.Collections.Generic;
using System.Linq;
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
    public class HomeController : ControllerBase
    {
        private readonly UserCore _userCore;

        public HomeController(UserCore userCore)
        {
            _userCore = userCore;
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<dynamic> Authenticate([FromBody] User model)
        {
            var user = _userCore.LoginUser(model, HttpContext.Connection.RemoteIpAddress);
            if (user == null)
                return BadRequest(new { message = "Usename or password invalid" });
            var token = TokenService.CreateToken(user);
            return new
            {
                user = user,
                token = token
            };
        }

        [HttpPost]
        [Route("alive")]
        [Authorize]
        public ActionResult<dynamic> IsAlive()
        {
            return new {
                message = "yes",
            };
        }

        [HttpPost]
        [Route("signUp")]
        [AllowAnonymous]
        public ActionResult<dynamic> SignUp([FromBody] User model)
        {

            var ipAddress = HttpContext.Connection.RemoteIpAddress;
            var completeUser = _userCore.CreateANewUser(model, ipAddress);
            if (completeUser == null)
                return BadRequest(new { message = "username already exist" });
            completeUser.Password = "";
            var token = TokenService.CreateToken(completeUser);
            return new
            {
                user = completeUser,
                token = token
            };
        }
    }
}