
import { CommenterRequest } from './commenterRequest';
import { CommentTypeEnum } from './Enums/commentTypeEnum';

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