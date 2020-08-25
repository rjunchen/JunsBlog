using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class Article : EntityBase
    {
        [BsonRequired]
        public string Title { get; set; }
        [BsonRequired]
        public string Content { get; set; }
        [BsonRequired]
        public string CoverImage { get; set; }
        [BsonRequired]
        public string Abstract { get; set; }
        [BsonRequired]
        public string AuthorId { get; set; }
        [BsonRequired]
        public string[] Categories { get; set; }
        [BsonRequired]
        public bool IsPrivate { get; set; }
        [BsonRequired]
        public bool IsAproved { get; set; }
        [BsonRequired]
        public int Views { get; set; }
        [BsonRequired]
        public DateTime UpdatedOn { get; set; }
        [BsonRequired]
        public DateTime CreatedOn { get; set; }
    }
}
