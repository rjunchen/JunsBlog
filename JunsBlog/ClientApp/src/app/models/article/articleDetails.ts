import { User } from '../authentication/user';
import { Article } from '../article/article';
import { ArticleRankingDetails } from './articleRankingDetails'

export class ArticleDetails extends Article{
    author: User;
    updatedOn: string;
    createdOn: string;
    views: number;
    content: string;
    IsApproved: boolean;
    commentsCount: number;
}