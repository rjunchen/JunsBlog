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
        [BsonRepresentation(BsonType.ObjectId)]
        public string TargetId { get; set; }
        [BsonRequired]
        [BsonRepresentation(BsonType.ObjectId)]
        public string CommenterId { get; set; }
        [BsonRequired]
        public CommentType CommentType { get; set; }

        public Comment(CommentRequest model, string userId)
        {
            CommentText = model.CommentText;
            TargetId = model.TargetId;
            CommenterId = userId;
            CommentType = model.CommentType;
        }
    }
}
