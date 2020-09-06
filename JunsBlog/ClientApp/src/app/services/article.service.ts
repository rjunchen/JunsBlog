import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Article } from '../models/article/article';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ArticleDetails } from '../models/article/articleDetails'
import { ArticleRankingDetails } from '../models/article/articleRankingDetails';
import { RankEnum } from '../models/enums/rankEnum';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  public saveArticle(article: Article): Observable<string>{
    return this.http.post('/api/article/save', article, {responseType: 'text'}).pipe(map(id => { return <string>id}));
  }

  public getArticle(articleId: string): Observable<Article>{
    return this.http.get(`/api/article/get?articleId=${articleId}`).pipe(map(data => { return <Article>data}));
  }

  getArticleDetails(articleId: string): Observable<ArticleDetails> {
    return this.http.get(`/api/article/details/get?articleId=${articleId}`).pipe(map(data => { return <ArticleDetails>data}));
  }

  public rankArticle(articleId: string, rank: RankEnum): Observable<ArticleRankingDetails>{
    return this.http.post('/api/article/rank', { articleId, rank }).pipe(map(data => { return <ArticleRankingDetails>data}));
  }

  public getArticleRankingDetails(articleId: string): Observable<ArticleRankingDetails>{
    return this.http.get(`/api/article/rank?articleId=${articleId}`).pipe(map(data => { return <ArticleRankingDetails>data}));
  }
}
