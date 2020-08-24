using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class Article
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]  // Convert MongoDB ObjectId type to string type
        public string Id { get; set; }
        [BsonRequired]
        public string Title { get; set; }
        [BsonRequired]
        public string Content { get; set; }
        [BsonRequired]
        public string CoverImage { get; set; }
        [BsonRequired]
        public string Abstract { get; set; }
        [BsonRequired]
        public DateTime CreationDate { get; set; }
        [BsonRequired]
        public string AuthorId { get; set; }
        [BsonRequired]
        public string[] Categories { get; set; }
        [BsonRequired]
        public bool IsPrivate { get; set; }
    }
}
