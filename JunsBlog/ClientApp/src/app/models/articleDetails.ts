import { User } from './user';
import { Article } from './article';
import { RankingRequest } from './RankingRequest';
import { RankingResponse } from './rankingResponse';

export class ArticleDetails extends Article{
    ranking: RankingResponse;
    author: User;
    updatedOn: string;
    views: number;
    content: string;
    id: string;
    comments: Array<string>;
}
