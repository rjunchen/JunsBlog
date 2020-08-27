import { CommentTypeEnum } from './CommentTypeEnum';
import { CommenterRequest } from './commenterRequest';

export class CommentRequest {
    targetId: string;
    commentText: string;
    commentType: CommentTypeEnum
    
    constructor(commenterRequest: CommenterRequest){
        this.targetId = commenterRequest.callerId;
        this.commentText = commenterRequest.commentDraft;
        this.commentType = commenterRequest.commentType;
    }
}