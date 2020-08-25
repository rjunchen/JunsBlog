﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Entities
{
    public class Comment : EntityBase
    {
        [BsonRequired]
        public DateTime UpdatedOn { get; set; }
        [BsonRequired]
        public DateTime CreatedOn { get; set; }
    }
}
