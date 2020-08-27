import { CommentDetails } from './commentDetails';
import { CommentTypeEnum } from './CommentTypeEnum';

export class CommenterRequest{
    callerId: string;
    comments: Array<CommentDetails>;
    commentType: CommentTypeEnum;
    commentDraft: string;

    constructor(callerId: string, comments: Array<CommentDetails>,  commentType: CommentTypeEnum, commentDraft: string = ""){
        if(!comments) comments = [];

        this.callerId = callerId;
        this.comments =  comments;  // Assign empty array if the comments is null
        this.commentType = commentType;
        this.commentDraft = commentDraft; 
    }
}
