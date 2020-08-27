import { Injectable } from '@angular/core';
import { Article } from '../models/article';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ArticleDetails } from '../models/articleDetails';
import { ArticleSearchPagingResult } from '../models/articleSearchPagingResult';
import { ArticleRankingRequest } from '../models/articleRankingRequest';
import { ArticleRankingDetails } from '../models/articleRankingDetails';
import { SortOrderEnum } from '../models/Enums/sortOrderEnum';
import { Observable } from 'rxjs';
import { SortByEnum } from '../models/Enums/sortByEnum';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  public createArticle(article: Article): Observable<ArticleDetails>{
    return this.http.post('/api/article/create', article).pipe(map(data => { return <ArticleDetails>data}));
  }

  public getArticle(articleId: string): Observable<ArticleDetails>{
    return this.http.get(`/api/article/get?articleId=${articleId}`).pipe(map(data => { return <ArticleDetails>data}));
  }

  public searchArticle(page: number, pageSize: number, searchKey: string = "", 
    sortBy: SortByEnum = SortByEnum.CreatedOn, sortOrder: SortOrderEnum = SortOrderEnum.descending) : Observable<ArticleSearchPagingResult> {
    return this.http.get(`/api/article/search?page=${page}&pageSize=${pageSize}&searchKey=${searchKey}
      &sortOrder=${sortOrder}&sortBy=${sortBy}`).pipe(map(data => { return <ArticleSearchPagingResult>data}));
  }

  public rankArticle(rankRequest: ArticleRankingRequest): Observable<ArticleRankingDetails>{
    return this.http.post('/api/article/rank', rankRequest).pipe(map(data => { return <ArticleRankingDetails>data}));
  }

  public GetArticleRanking(articleId: string): Observable<ArticleRankingDetails>{
    return this.http.get(`/api/article/rank?articleId=${articleId}`).pipe(map(data => { return <ArticleRankingDetails>data}));
  }
}
