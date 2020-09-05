import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Article } from '../models/article/article';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { ArticleDetails } from '../models/article/articleDetails'

@Injectable({
  providedIn: 'root'
})
export class ArticleService {
 
  constructor(private http: HttpClient) { }

  public saveArticle(article: Article): Observable<Article>{
    return this.http.post('/api/article/save', article).pipe(map(data => { return <Article>data}));
  }

  public getArticle(articleId: string): Observable<Article>{
    return this.http.get(`/api/article/get?articleId=${articleId}`).pipe(map(data => { return <Article>data}));
  }

  getArticleDetails(articleId: string): Observable<ArticleDetails> {
    return this.http.get(`/api/article/details/get?articleId=${articleId}`).pipe(map(data => { return <ArticleDetails>data}));
  }
}
