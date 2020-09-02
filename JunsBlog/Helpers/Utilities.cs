using HtmlAgilityPack;
using JunsBlog.Entities;
using JunsBlog.Models.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Helpers
{
    public static class Utilities
    {
        public static string GenerateToken()
        {
            return Guid.NewGuid().ToString();
        }

        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public static bool ValidatePassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }


        public static void MassageArticleImages(Article article)
        {
            if (String.IsNullOrWhiteSpace(article.Content)) return;
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(article.Content);
            var nodes = htmlDoc.DocumentNode.Descendants("img");

            foreach (var node in nodes)
            {
                var srcAttribute = node.Attributes.FirstOrDefault(x => x.Name.Equals("src", StringComparison.OrdinalIgnoreCase));
                article.GalleryImages.Add(new GalleryImage(srcAttribute.Value, srcAttribute.Value, srcAttribute.Value));              
            }
        }
    }
}
