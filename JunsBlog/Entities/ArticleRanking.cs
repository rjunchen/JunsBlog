using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class ArticleRanking : EntityBase
    {
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ArticleId { get; set; }
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
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

        public ArticleRanking(string articleId, string userId)
        {
            UserId = userId;
            ArticleId = articleId;
        }
    }
}
