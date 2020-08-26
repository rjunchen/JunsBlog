import { SortOrderEnum } from './sortOrderEnum';
import { SortByEnum } from './sortByEnum';

export interface InterfaceSearchPagingResult {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortOrder: SortOrderEnum;
    sortBy: SortByEnum;
    documents: Array<any>
}