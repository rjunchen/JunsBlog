import { Article } from './article';
import { ArticleRankingRequest } from './articleRankingRequest';
import { ArticleRankingDetails } from './articleRankingDetails';
import { User } from '../user';

export class ArticleDetails extends Article{
    author: User;
    updatedOn: string;
    createdOn: string;
    views: number;
    content: string;
    IsApproved: boolean;
    commentsCount: number;
}
