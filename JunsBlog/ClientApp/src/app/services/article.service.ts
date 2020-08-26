import { Injectable } from '@angular/core';
import { Article } from '../models/article';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ArticleDetails } from '../models/articleDetails';
import { ArticlePagingResult } from '../models/articlePagingResult';
import { RankingRequest } from '../models/RankingRequest';
import { RankingResponse } from '../models/rankingResponse';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  public createArticle(article: Article){
    return this.http.post('/api/article/create', article).pipe(map(data => { return <ArticleDetails>data}));
  }

  public getArticle(articleId: string){
    return this.http.get(`/api/article/get?articleId=${articleId}`).pipe(map(data => { return <ArticleDetails>data}));
  }

  public searchArticle(page: number, pageSize: number, searchKey: string = "", sortBy: string = "updatedOn", sortOrder: number = 1){
    return this.http.get(`/api/article/search?page=${page}&pageSize=${pageSize}&searchKey=${searchKey}
      &sortOrder=${sortOrder}&sortBy=${sortBy}`).pipe(map(data => { return <ArticlePagingResult>data}));
  }

  public rankArticle(rankRequest: RankingRequest){
    return this.http.post('/api/article/rank', rankRequest).pipe(map(data => { return <RankingResponse>data}));
  }

  public GetArticleRanking(articleId: string){
    return this.http.get(`/api/article/rank?articleId=${articleId}`).pipe(map(data => { return <RankingResponse>data}));
  }
}
