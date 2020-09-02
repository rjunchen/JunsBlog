using JunsBlog.Models.Articles;
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
        [BsonRepresentation(BsonType.ObjectId)]
        public string AuthorId { get; set; }
        [BsonRequired]
        public string[] Categories { get; set; }
        [BsonRequired]
        public bool IsPrivate { get; set; }
        [BsonRequired]
        public bool IsApproved { get; set; }
        [BsonRequired]
        public int Views { get; set; }
        public List<GalleryImage> GalleryImages { get; set; }

        public Article(ArticleRequest model, string authorId)
        {
            Abstract = model.Abstract;
            AuthorId = authorId;
            Content = model.Content;
            CoverImage = model.CoverImage;
            IsPrivate = model.IsPrivate;
            Title = model.Title;
            Categories = model.Categories;
            GalleryImages = new List<GalleryImage>();
        }

        public void UpdateContents(ArticleRequest model)
        {
            Abstract = model.Abstract;
            Content = model.Content;
            CoverImage = model.CoverImage;
            IsPrivate = model.IsPrivate;
            Title = model.Title;
            Categories = model.Categories;
         }
    }
}
