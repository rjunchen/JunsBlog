import { Injectable } from '@angular/core';
import { Article } from '../models/article';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { ArticleDetails } from '../models/articleDetails';

@Injectable({
  providedIn: 'root'
})
export class ArticleService {

  constructor(private http: HttpClient) { }

  public CreateArticle(article: Article){
    return this.http.post('/api/article/post', article).pipe(map(data => { return <ArticleDetails>data}));
  }

  public GetArticle(articleId: string){
    return this.http.get(`/api/article/get?articleId=${articleId}`).pipe(map(data => { return <ArticleDetails>data}));
  }
}
