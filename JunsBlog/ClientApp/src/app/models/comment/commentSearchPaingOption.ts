import { SortOrderEnum } from '../Enums/sortOrderEnum';
import { SortByEnum } from '../Enums/sortByEnum';
import { commentSearchOnEnum } from '../Enums/commentSearchOnEnum';


export class CommentSearchPagingOption {
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortBy: SortByEnum;
    sortOrder: SortOrderEnum;
    searchOn: commentSearchOnEnum;
    constructor(currentPage: number = 1, pageSize: number = 10, searchKey: string = null,   searchOn: commentSearchOnEnum = commentSearchOnEnum.CommentText,
         sortBy: SortByEnum = SortByEnum.CreatedOn, sortOrder: SortOrderEnum = SortOrderEnum.descending){
            this.currentPage = currentPage;
            this.pageSize = pageSize;
            this.searchKey = searchKey;
            this.sortBy = sortBy;
            this.sortOrder = sortOrder;
            this.searchOn = searchOn;
    }
}