import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { Article } from '../models/article/article';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ArticleDetails } from '../models/article/articleDetails'
import { ArticleRankingDetails } from '../models/article/articleRankingDetails';
import { RankEnum } from '../models/enums/rankEnum';
import { ArticleSearchPagingOption } from '../models/article/articleSearchPagingOption'
import { ArticleSearchPagingResult } from '../models/article/articleSearchPagingResult'

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
  @Output() onSearchClicked: EventEmitter<ArticleSearchPagingOption> = new EventEmitter();
  constructor(private http: HttpClient) { }

  public SearchClicked(option: ArticleSearchPagingOption){
    this.onSearchClicked.emit(option);
  }

  public saveArticle(article: Article): Observable<any>{
    return this.http.post('/api/article/save', article).pipe(map(id => { return id}));
  }

  public getArticle(articleId: string): Observable<Article>{
    return this.http.get<Article>(`/api/article/get?articleId=${articleId}`);
  }

  getArticleDetails(articleId: string): Observable<ArticleDetails> {
    return this.http.get<ArticleDetails>(`/api/article/details/get?articleId=${articleId}`);
  }

  public rankArticle(articleId: string, rank: RankEnum): Observable<ArticleRankingDetails>{
    return this.http.post<ArticleRankingDetails>('/api/article/rank', { articleId, rank });
  }

  public getArticleRankingDetails(articleId: string): Observable<ArticleRankingDetails>{
    return this.http.get<ArticleRankingDetails>(`/api/article/rank?articleId=${articleId}`);
  }
 
  public searchArticle(option: ArticleSearchPagingOption) : Observable<ArticleSearchPagingResult> {
    return this.http.post<ArticleSearchPagingResult>('/api/article/search', option);
  }
}
