using System;
using System.Net;
using System.Threading.Tasks;
using System.Collections.Generic;
using netart.Models;
using netart.Services;
using netart.Utilities;

namespace netart.LogicalCore
{
    public class PostCore
    {
        private readonly UserService _userService;
        private readonly UserCore _userCore;
        private readonly PostService _postService;
        // using DEpendency injectiojn to get the service with the connection to the database
        public PostCore(UserService userService, UserCore userCore, PostService postService)
        {
            _userService = userService;
            _userCore = userCore;
            _postService = postService;
        }
        public List<Post> GetSubscribePost(string username)
        {
            var result = new List<Post>();
            var subscribes = _userCore.GetSubscribesName(username);
            if (subscribes == null)
                return null;
            foreach (var subscribe in subscribes)
            {
                var temp = _postService.GetFrom(subscribe);
                if (temp.Count == 0)
                    continue;
                result.AddRange(temp);
            }
            if (result.Count == 0)
                return null;
            return result;
        }

        public async Task<List<Post>> GetLastPost(int nb)
        {
            return await _postService.GetLast(nb);
        }
        public Post CreateNew(Post post)
        {
            post.CreationDate = DateTime.UtcNow;
            post.Like = 0;
            post.CommentList = new List<string>();
            if (post.FileType != null && post.FileType != "none") {
                post.FileUuid = Guid.NewGuid().ToString();
            }
            return _postService.Create(post);
        }

        public List<Post> GetMyPost(string username)
        {
            return _postService.GetFrom(username);
        }
    }
}