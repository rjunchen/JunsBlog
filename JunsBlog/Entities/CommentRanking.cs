using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class CommentRanking : EntityBase
    {
        [BsonRequired]
        public string CommentId { get; set; }
        [BsonRequired]
        public string UserId { get; set; }
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool DidILike { get; set; }
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool DidIDislike { get; set; }
        [BsonRequired]
        [BsonDefaultValue(false)]
        public bool DidIFavor { get; set; }

        public CommentRanking(string commentId, string currentUserId)
        {
            this.CommentId = commentId;
            this.UserId = currentUserId;
        }
    }
}
