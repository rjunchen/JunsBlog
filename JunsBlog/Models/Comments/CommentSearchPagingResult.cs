using JunsBlog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Comments
{
    public class CommentSearchPagingResult
    {
        public List<CommentDetails> Documents { get; set; }
        public int TotalDocuments { get; set; }
        public int TotalPages { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public CommentSearchPagingOption SearchOption { get; set; }

        public CommentSearchPagingResult(List<CommentDetails> commentDetailsList, int totalDocument, CommentSearchPagingOption options)
        {
            Documents = commentDetailsList;
            TotalDocuments = totalDocument;
            TotalPages = (int)Math.Ceiling((double)totalDocument / options.PageSize);
            HasNextPage = TotalPages > options.CurrentPage;
            HasPreviousPage = options.CurrentPage > 1;
            SearchOption = options;
        }
    }
}
