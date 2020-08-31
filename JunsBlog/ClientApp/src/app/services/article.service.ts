import { Injectable, EventEmitter, Output } from '@angular/core';
import { Article } from '../models/article/article';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ArticleSearchPagingOption } from '../models/article/articleSearchPagingOption';
import { ArticleDetails } from '../models/article/articleDetails';
import { ArticleSearchPagingResult } from '../models/article/articleSearchPagingResult';
import { ArticleRankingDetails } from '../models/article/articleRankingDetails';
import { ArticleRankingRequest } from '../models/article/articleRankingRequest';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  @Output() onSearchClicked: EventEmitter<ArticleSearchPagingOption> = new EventEmitter();
  constructor(private http: HttpClient) { }

  public SearchClicked(option: ArticleSearchPagingOption){
    this.onSearchClicked.emit(option);
  }

  public createArticle(article: Article): Observable<ArticleDetails>{
    return this.http.post('/api/article/create', article).pipe(map(data => { return <ArticleDetails>data}));
  }

  public UpdateArticle(article: Article): Observable<Article>{
    return this.http.post('/api/article/update', article).pipe(map(data => { return <Article>data}));
  }

  public getArticleDetails(articleId: string): Observable<ArticleDetails>{
    return this.http.get(`/api/article/details/get?articleId=${articleId}`).pipe(map(data => { return <ArticleDetails>data}));
  }

  public getArticle(articleId: string): Observable<Article>{
    return this.http.get(`/api/article/get?articleId=${articleId}`).pipe(map(data => { return <Article>data}));
  }

  public searchArticle(option: ArticleSearchPagingOption) : Observable<ArticleSearchPagingResult> {
    return this.http.post('/api/article/search', option).pipe(map(data => { return <ArticleSearchPagingResult>data}));
  }

  public rankArticle(rankRequest: ArticleRankingRequest): Observable<ArticleRankingDetails>{
    return this.http.post('/api/article/rank', rankRequest).pipe(map(data => { return <ArticleRankingDetails>data}));
  }

  public GetArticleRanking(articleId: string): Observable<ArticleRankingDetails>{
    return this.http.get(`/api/article/rank?articleId=${articleId}`).pipe(map(data => { return <ArticleRankingDetails>data}));
  }
}
