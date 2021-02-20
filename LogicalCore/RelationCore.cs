using System;
using System.Net;
using System.Collections.Generic;
using netart.Models;
using netart.Services;
using netart.Utilities;

namespace netart.LogicalCore
{
    public class RelationCore
    {
        private readonly UserService _userService;
        private readonly UserCore _userCore;
        private readonly PostService _postService;
        private readonly PostCore _postCore;
        // using DEpendency injectiojn to get the service with the connection to the database
        public RelationCore(UserService userService, UserCore userCore,
                            PostService postService, PostCore postCore)
        {
            _userService = userService;
            _userCore = userCore;
            _postService = postService;
            _postCore = postCore;
        }

        public UserCompte NewFollowing(string follower, string subscribe)
        {
            var subscribeDb = _userService.GetCompte(subscribe);
            if (subscribeDb == null)
                return null;
            var followerDb = _userService.GetCompte(follower);
            if (followerDb == null)
                return null;
            if (followerDb.Subscribe == null)
                followerDb.Subscribe = new List<string>();
            if (subscribeDb.Subscribe == null)
                subscribeDb.Subscribe = new List<string>();
            followerDb.Subscribe.Add(subscribe);
            subscribeDb.Follower.Add(follower);
            _userService.UpdateFollower(subscribeDb);
            _userService.UpdateSubscribe(followerDb);
            followerDb.Password = "";
            return followerDb;
        }

        public UserCompte GetUserCompte(string username)
        {
            var dbUser = _userService.GetCompte(username);
            if (dbUser == null)
                return null;
            return dbUser;
        }
    }
}