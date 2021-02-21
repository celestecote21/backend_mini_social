using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace netart.Models
{
    public class UserCompte : User
    {
        public List<string> PostList { set; get; }
        public List<string> Follower { set; get; }
        public List<string> Subscribe { set; get; }

        public UserCompte Hide()
        {
            this.Password = null;
            this.BdId = null;
            return this;
        }
    }
}
