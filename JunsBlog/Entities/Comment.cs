using JunsBlog.Models.Comments;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class Comment : EntityBase
    {
        [BsonRequired]
        public string CommentText { get; set; }
        [BsonRequired]
        public string ArticleId { get; set; }
        [BsonRequired]
        public string UserId { get; set; }
        [BsonRequired]
        public string ParentId { get; set; }

        public Comment(CommentRequest model, string userId)
        {
            CommentText = model.CommentText;
            ArticleId = model.ArticleId;
            UserId = userId;
            ParentId = model.ParentId;
        }
    }
}
