﻿using JunsBlog.Entities;
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
        public string Abstract { get; set; }
        public User Author { get; set; }
        public string[] Categories { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsApproved { get; set; }
        public int Views { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
        public int Favors { get; set; }
        public int CommentsCount { get; set; }
        public DateTime UpdatedOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public List<GalleryImage> GalleryImages { get; set; }
        public ArticleRankingDetails Ranking { get; set; }
    }
}
