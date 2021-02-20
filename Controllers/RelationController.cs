
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
    public class RelationController : ControllerBase
    {

        private readonly RelationCore _relationCore;
        public RelationController(RelationCore relationCore)
        {
            _relationCore = relationCore;
        }

        [HttpGet]
        [Route("add/{subscribe}")]
        [Authorize]
        public ActionResult<dynamic> AddSubscribe(string subscribe)
        {
            var follower = HttpContext.User.FindFirst(ClaimTypes.Name).Value;
            var updatedUser = _relationCore.NewFollowing(follower, subscribe);
            if (updatedUser == null)
                return BadRequest(new {message = "cannot subscribe to this people"});
            else
                return updatedUser;
        }

        [HttpGet]
        [Route("get/{user}")]
        [Authorize]
        public ActionResult<dynamic> GetAUserCompte(string user)
        {
            var userToSend = _relationCore.GetUserCompte(user);
            if (userToSend == null)
                return BadRequest(new {message = "the user you want don't existe"});
            else
                return userToSend;
        }
    }
}