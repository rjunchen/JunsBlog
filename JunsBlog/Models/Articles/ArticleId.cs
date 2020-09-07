using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleId
    {
        public string Id { get; set; }

        public ArticleId(string id)
        {
            Id = id;
        }
    }
}
