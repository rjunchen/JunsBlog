using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Text.Json.Serialization;

namespace JunsBlog.Entities
{
    public class User : EntityBase
    {
        [BsonRequired]
        public string Name { get; set; }
        [BsonRequired]
        public string Email { get; set; }
        [BsonRequired]
        public string Role { get; set; }
        [BsonRequired]
        public string Type { get; set; }
        [BsonRequired]
        public string Image { get; set; }
        [JsonIgnore]
        public string Password { get; set; }
    }
}
