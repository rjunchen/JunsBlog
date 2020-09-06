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
        public List<string> Likes { get; set; }
        public List<string> Dislikes { get; set; }
        public List<string> Favors { get; set; }

        public ArticleRanking(string articleId)
        {
            Id = articleId;
            Likes = new List<string>();
            Dislikes = new List<string>();
            Favors = new List<string>();
        }
    }
}
