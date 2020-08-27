import { User } from './user';
import { CommentRankingDetails } from './commentRankingDetails';

export class CommentDetails{
    ranking: CommentRankingDetails;
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
