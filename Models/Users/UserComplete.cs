using System;
using System.Net;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace netart.Models
{
    public class UserComplete: UserCompte
    {
        public UserCompte ToCompte()
        {
            if (this == null)
                return null;
            return new UserCompte{
                Username = this.Username,
                Follower = this.Follower,
                Subscribe = this.Subscribe,
                Role = this.Role,
                PostList = this.PostList
            };
        }

        public User ToUser()
        {
            if(this == null)
                return null;
            return new User {
                Username = this.Username,
                Role = this.Role
            };
        }
        public string Ip
        {
            set;
            get;
        }
        public DateTime RegistrationDate
        {
            set;
            get;
        }
    }
}