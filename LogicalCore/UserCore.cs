using System;
using System.Net;
using System.Collections.Generic;
using netart.Models;
using netart.Services;
using netart.Utilities;

namespace netart.LogicalCore
{
    public class UserCore
    {
        private readonly UserService _userService;
        // using DEpendency injectiojn to get the service with the connection to the database
        public UserCore(UserService userService)
        {
            _userService = userService;
        }
        public UserComplete CreateANewUser(User model, IPAddress ip)
        {
            var newUser = new UserComplete();
            if (_userService.Get(model.Username) != null)
                return null;
            newUser.Password = new PasswordSha256().create_sha256(model.Password);
            newUser.Username = model.Username;
            newUser.Role = "user";
            newUser.RegistrationDate = DateTime.UtcNow;
            newUser.PostList = new List<string>();
            newUser.Follower = new List<string>();
            newUser.Ip = ip.ToString();
            _userService.Create(newUser);
            return newUser;
        }
        public User LoginUser(User model, IPAddress ip)
        {
            var dbUser = _userService.GetComplete(model.Username);
            if (dbUser == null)
                return null;
            var passwordShat = new PasswordSha256().create_sha256(model.Password);
            if (dbUser.Password != passwordShat)
                return null;
            dbUser.ToCompte();
            return dbUser;
        }

        public List<string> GetFollowerName(string username)
        {
            var dbUser = _userService.GetCompte(username);
            var result = new List<string>();
            if (dbUser == null)
                return null;
            dbUser.Follower.ForEach(follower => result.Add(follower));
            return result;
        }
        public List<string> GetSubscribesName(string username)
        {
            var dbUser = _userService.GetCompte(username);
            var result = new List<string>();
            if (dbUser == null)
                return null;
            result = dbUser.Subscribe;
            if (result == null)
                return null;
            return result;
        }
    }
}