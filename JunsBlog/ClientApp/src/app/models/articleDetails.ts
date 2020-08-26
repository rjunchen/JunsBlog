import { User } from './user';
import { Article } from './article';
import { RankingRequest } from './RankingRequest';
import { ArticleRankingDetails } from './articleRankingDetails';
import { CommentDetails } from './commentDetails';

export class ArticleDetails extends Article{
    author: User;
    updatedOn: string;
    createdOn: string;
    views: number;
    content: string;
    id: string;
    IsApproved: boolean;
    comments: Array<CommentDetails>;
}
