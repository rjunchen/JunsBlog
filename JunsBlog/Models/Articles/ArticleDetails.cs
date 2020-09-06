using JunsBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleDetails
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public User Author { get; set; }
        public string[] Categories { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsApproved { get; set; }
        public int Views { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
