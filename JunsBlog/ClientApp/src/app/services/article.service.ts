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

  public createArticle(article: Article): Observable<ArticleDetails>{
    return this.http.post('/api/article/create', article).pipe(map(data => { return <ArticleDetails>data}));
  }

  public UpdateArticle(article: Article): Observable<Article>{
    return this.http.post('/api/article/update', article).pipe(map(data => { return <Article>data}));
  }
  
}
