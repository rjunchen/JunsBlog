import { CommentDetails } from './commentDetails';

export class CommenterRequest{
    callerId: string;
    comments: Array<CommentDetails>;
    isCallerArticle: boolean;

    constructor(callerId: string, comments: Array<CommentDetails>, isCallerArticle: boolean){
        this.callerId = callerId;
        this.comments = comments;
        this.isCallerArticle = isCallerArticle;
    }
}
