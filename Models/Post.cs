using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace netart.Models
{
    public class Post
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BdId
        {
            get;
            set;
        }
        [BsonElement("UserName")]
        public string Username
        {
            get;
            set;
        }
        public string Title
        {
            get;
            set;
        }
        public string Content 
        {
            get;
            set;
        }
        public DateTime CreationDate
        {
            get;
            set;
        }
    }
}