import { User } from './user';
import { CommentRankingDetails } from './commentRankingDetails';

export class CommentDetails{
    ranking: CommentRankingDetails;
    user: User;
    commentText: string;
    updatedOn: string;
    content: string;
    articleId: string;
    id: string;
    parentId: string;
    childrenCommentsCount: number;
}
