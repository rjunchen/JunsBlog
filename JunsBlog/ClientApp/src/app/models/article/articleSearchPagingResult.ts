import { ArticleDetails } from './articleDetails';
import { ArticleSearchPagingOption } from './articleSearchPagingOption';

export class ArticleSearchPagingResult  {
    hasNextPage: boolean;
    hasPrevPage: boolean;
    totalDocuments: number;
    totalPages: number;
    searchOption: ArticleSearchPagingOption
    documents: ArticleDetails[];
}