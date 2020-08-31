using JunsBlog.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JunsBlog.Models.Articles
{
    public class ArticleSearchPagingOption
    {
        public string SearchKey { get; set; }
        public SortOrderEnum SortOrder { get; set; }
        public SortByEnum SortBy { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public ArticleFilterEnum Filter { get; set; }

        public ArticleSearchPagingOption()
        {
            SearchKey = String.Empty;
            SortOrder = SortOrderEnum.Descending;
            SortBy = SortByEnum.UpdatedOn;
            CurrentPage = 1;
            PageSize = 10;
            Filter = ArticleFilterEnum.All;
        }
    }
}
