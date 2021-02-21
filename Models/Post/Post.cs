using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace netart.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BdId { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string FileType { get; set; }
        public string FileUuid { get; set; }
        public int Like { get; set; }
        public List<string> CommentList { get; set; }
        public DateTime CreationDate { get; set; }

        public Post Hide()
        {
            this.BdId = null;
            return this;
        }
    }
}