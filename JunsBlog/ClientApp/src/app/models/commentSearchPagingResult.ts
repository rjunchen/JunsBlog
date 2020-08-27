import { InterfaceSearchPagingResult } from './InterfaceSearchPagingResult';
import { SortOrderEnum } from './Enums/sortOrderEnum';
import { CommentDetails } from './commentDetails';
import { SortByEnum } from './Enums/sortByEnum';
import { commentSearchOnEnum } from './Enums/commentSearchOnEnum';

export class CommentSearchPagingResult implements InterfaceSearchPagingResult {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortOrder: SortOrderEnum;
    sortBy: SortByEnum;
    searchOn: commentSearchOnEnum;
    documents: CommentDetails[];
}