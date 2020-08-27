import { InterfaceSearchPagingResult } from './InterfaceSearchPagingResult';
import { ArticleDetails } from './articleDetails';
import { SortOrderEnum } from './Enums/sortOrderEnum';
import { SortByEnum } from './Enums/sortByEnum';

export class ArticleSearchPagingResult implements InterfaceSearchPagingResult {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortOrder: SortOrderEnum;
    sortBy: SortByEnum;
    documents: ArticleDetails[];
}