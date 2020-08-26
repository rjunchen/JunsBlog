import { InterfaceSearchPagingResult } from './InterfaceSearchPagingResult';
import { ArticleDetails } from './articleDetails';
import { SortByEnum } from './sortByEnum';
import { SortOrderEnum } from './sortOrderEnum';

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