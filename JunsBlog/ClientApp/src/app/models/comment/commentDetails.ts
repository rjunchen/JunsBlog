import { CommentRankingDetails } from "./commentRankingDetails";
import { User } from '../../models/authentication/user'

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
