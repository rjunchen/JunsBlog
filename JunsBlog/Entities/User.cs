using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System;

namespace JunsBlog.Entities
{
    public class User
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]  // Convert MongoDB ObjectId type to string type
        public string Id { get; set; }
        [BsonRequired]
        public string Name { get; set; }
        [BsonRequired]
        public string Email { get; set; }
        [BsonRequired]
        public string Role { get; set; }
        [BsonRequired]
        public string Type { get; set; }
        [BsonRequired]
        public DateTime CreationDate { get; set; }
        public string Image { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
