import { CommentDetails } from './commentDetails';
import { SortOrderEnum } from '../Enums/sortOrderEnum';
import { SortByEnum } from '../Enums/sortByEnum';
import { commentSearchOnEnum } from '../Enums/commentSearchOnEnum';
import { CommentSearchPagingOption } from './commentSearchPaingOption';


export class CommentSearchPagingResult {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    searchOption: CommentSearchPagingOption;
    documents: CommentDetails[];
}