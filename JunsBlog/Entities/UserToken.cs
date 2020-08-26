using JunsBlog.Helpers;
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
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        public string ResetToken { get; set; }
        public DateTime ResetExpiry { get; set; }

        public UserToken(string userId)
        {
            RefreshToken = Utilities.GenerateToken();
            RefreshExpiry = DateTime.UtcNow.AddDays(14);
            UserId = userId;
        }

        public void CreateResetToken()
        {
            ResetToken = Utilities.GenerateToken();
            ResetExpiry = DateTime.UtcNow.AddMinutes(10);
        }
    }
}
