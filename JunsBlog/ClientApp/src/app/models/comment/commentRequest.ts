import { CommentDetails } from './commentDetails';
import { User } from '../user';
import { ArticleDetails } from '../article/articleDetails';

export class CommentRequest {
    articleId: string;
    commentText: string;
    parentId: string;
    replyToUser: User;
    
    constructor(articleDetails: ArticleDetails, commentDetails: CommentDetails = null){
        this.articleId = articleDetails.id;
        this.commentText = "";
        this.replyToUser = commentDetails ? ( commentDetails.parentId == commentDetails.articleId ? null : commentDetails.user ) : null;
        // we don't want nested comments, so comment on a child comment, it will reply to its parent comment instead
        this.parentId = commentDetails ? ( commentDetails.parentId == commentDetails.articleId ? commentDetails.id : commentDetails.parentId ) : articleDetails.id;
    }
}