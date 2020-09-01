using JunsBlog.Entities;
using JunsBlog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleSearchPagingResult
    {
        public List<ArticleDetails> Documents { get; set; }
        public int TotalDocuments { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public ArticleSearchPagingOption SearchOption { get; set; }

        public ArticleSearchPagingResult(List<ArticleDetails> articleDetailsList, int totalDocument, ArticleSearchPagingOption options)
        {
            Documents = articleDetailsList;
            TotalDocuments = totalDocument;
            TotalPages = (int)Math.Ceiling((double)totalDocument / options.PageSize);
            HasNextPage = TotalPages > options.CurrentPage;
            HasPreviousPage = options.CurrentPage > 1;
            SearchOption = options;
            CurrentPage = options.CurrentPage;
            PageSize = options.PageSize;
        }
    }
}
