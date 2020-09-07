using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class EntityBase
    {
        [BsonId]
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
