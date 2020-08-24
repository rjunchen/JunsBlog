import { InterfacePagingResult } from './InterfacePagingResult';
import { ArticleDetails } from './articleDetails';

export class ArticlePagingResult implements InterfacePagingResult {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    currentPage: number;
    pageSize: number;
    searchKey: string;
    sortOrder: string;
    sortBy: string;
    documents: ArticleDetails[];
}