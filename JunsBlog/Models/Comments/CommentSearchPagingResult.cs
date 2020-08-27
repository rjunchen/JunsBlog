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
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPreviousPage { get; set; }
        public string SearchKey { get; set; }
        public SortOrderEnum SortOrder { get; set; }
        public SortByEnum SortBy { get; set; }
        public CommentSearchOnEnum SearchOn { get; set; }

        public CommentSearchPagingResult(List<CommentDetails> commentDetailsList, int totalDocument, int currentPage, int pageSize, string searchKey, CommentSearchOnEnum searchOn, SortByEnum sortBy, SortOrderEnum sortOrder)
        {
            Documents = commentDetailsList;
            TotalDocuments = totalDocument;
            TotalPages = (int)Math.Ceiling((double)totalDocument / pageSize);
            CurrentPage = currentPage;
            PageSize = pageSize;
            HasNextPage = TotalPages > CurrentPage;
            HasPreviousPage = CurrentPage > 1;
            SearchKey = searchKey;
            SortOrder = sortOrder;
            SortBy = sortBy;
            SearchOn = searchOn;
        }
    }
}
