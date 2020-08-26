using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class EntityBase
    {
        [BsonIgnoreIfDefault]
        [BsonRepresentation(BsonType.ObjectId)]  // Convert MongoDB ObjectId type to string type
        public string Id { get; set; }
        [BsonRequired]
        public DateTime UpdatedOn { get; set; }
        [BsonRequired]
        public DateTime CreatedOn { get; set; }

        public EntityBase()
        {
            Id = ObjectId.GenerateNewId().ToString();
            UpdatedOn = DateTime.UtcNow;
            CreatedOn = DateTime.UtcNow;
        }
    }
}
