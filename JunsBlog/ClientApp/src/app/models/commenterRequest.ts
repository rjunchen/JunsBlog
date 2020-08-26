import { CommentDetails } from './commentDetails';

export class CommenterRequest{
    callerId: string;
    comments: Array<CommentDetails>;
    isCallerArticle: boolean;
}
