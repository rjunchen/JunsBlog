using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace JunsBlog.Entities
{
    public class UserToken : EntityBase
    {
        [BsonRequired]
        public string RefreshToken { get; set; }
        [BsonRequired]
        public DateTime RefreshExpiry { get; set; }
        [BsonRequired]
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime ResetExpiry { get; set; }
    }
}
