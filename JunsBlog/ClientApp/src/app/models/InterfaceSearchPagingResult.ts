import { SortOrderEnum } from './Enums/sortOrderEnum';
import { SortByEnum } from './Enums/sortByEnum';


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