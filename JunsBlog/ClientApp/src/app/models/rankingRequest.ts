import { RankEnum } from './Enums/rankEnum';


export class RankingRequest {
    articleId: string;
    rank: RankEnum;

    constructor(articleId: string, rank: RankEnum){
        this.articleId = articleId;
        this.rank = rank;
    }
}