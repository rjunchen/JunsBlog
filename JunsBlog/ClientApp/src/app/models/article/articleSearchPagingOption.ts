import { SortOrderEnum } from '../enums/sortOrderEnum';
import { SortByEnum } from '../enums/sortByEnum';
import { ArticleFilterEnum } from '../enums/articleFilterEnum';


export class ArticleSearchPagingOption {
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortBy: SortByEnum;
    sortOrder: SortOrderEnum;
    filter: ArticleFilterEnum;
    profilerId: string;

    constructor(currentPage: number = 1, pageSize: number = 10, searchKey: string = null, 
        sortBy: SortByEnum = SortByEnum.CreatedOn, sortOrder: SortOrderEnum = SortOrderEnum.descending,
         filter: ArticleFilterEnum = ArticleFilterEnum.All, profilerId: string = null){
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.searchKey = searchKey;
            this.sortBy = sortBy;
            this.sortOrder = sortOrder;
            this.filter = filter;
            this.profilerId = profilerId;
    }
}