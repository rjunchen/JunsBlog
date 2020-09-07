import { CommentDetails } from './commentDetails';
import { CommentSearchPagingOption } from './commentSearchPagingOption';

export class CommentSearchPagingResult {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    searchOption: CommentSearchPagingOption;
    documents: CommentDetails[];
}