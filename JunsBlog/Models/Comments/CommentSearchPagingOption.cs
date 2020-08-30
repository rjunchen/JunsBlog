using JunsBlog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Comments
{
    public class CommentSearchPagingOption
    {
        public string SearchKey { get; set; }
        public SortOrderEnum SortOrder { get; set; }
        public SortByEnum SortBy { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public CommentSearchOnEnum SearchOn { get; set; }

        public CommentSearchPagingOption()
        {
            SearchKey = String.Empty;
            SortOrder = SortOrderEnum.Descending;
            SortBy = SortByEnum.UpdatedOn;
            SearchOn = CommentSearchOnEnum.CommentText;
            CurrentPage = 1;
            PageSize = 10;
        }
    }
}
