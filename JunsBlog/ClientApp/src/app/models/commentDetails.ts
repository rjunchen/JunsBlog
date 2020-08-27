import { User } from './user';
import { CommentRankingResponse } from './commentRankingResponse';

export class CommentDetails{
    ranking: CommentRankingResponse;
    commenter: User;
    commentText: string;
    updatedOn: string;
    content: string;
    targetId: string;
    id: string;
    commentsCount: number;
    commentDraft: string;
    comments: Array<CommentDetails>;

}
