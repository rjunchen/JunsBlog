import { RankEnum } from '../Enums/rankEnum';


export class CommentRankingRequest {
    commentId: string;
    rank: RankEnum;

    constructor(commentId: string, rank: RankEnum){
        this.commentId = commentId;
        this.rank = rank;
    }
}