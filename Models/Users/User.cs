using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace netart.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string BdId
        {
            get;
            set;
        }

        public string Username
        {
            get;
            set;
        }
        public string Password
        {
            get;
            set;
        }
        public string Role
        {
            get;
            set;
        }
    }
}
