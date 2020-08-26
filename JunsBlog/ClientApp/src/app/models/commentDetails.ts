import { User } from './user';
import { CommentRankingResponse } from './commentRankingResponse';

export class CommentDetails{
    ranking: CommentRankingResponse;
    commenter: User;
    updatedOn: string;
    content: string;
    id: string;
    commentsCount: number;
    comments: Array<CommentDetails>;

}
