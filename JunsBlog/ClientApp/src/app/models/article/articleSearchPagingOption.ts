import { SortOrderEnum } from '../Enums/sortOrderEnum';
import { SortByEnum } from '../Enums/sortByEnum';


export class ArticleSearchPagingOption {
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortBy: SortByEnum;
    sortOrder: SortOrderEnum;
    constructor(currentPage: number = 1, pageSize: number = 10, searchKey: string = null, 
        sortBy: SortByEnum = SortByEnum.CreatedOn, sortOrder: SortOrderEnum = SortOrderEnum.descending){
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.searchKey = searchKey;
            this.sortBy = sortBy;
            this.sortOrder = sortOrder;
    }
}