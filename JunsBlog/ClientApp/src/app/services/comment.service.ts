import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CommentRequest } from '../models/commentRequest';
import { CommenterRequest } from '../models/commenterRequest';
import { CommentDetails } from '../models/commentDetails';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  @Output() onShowCommentControl: EventEmitter<any> = new EventEmitter();
  constructor(private http: HttpClient) { }

  public replyArticle(commentRequest: CommentRequest): Observable<CommentDetails> {
    return this.http.post('/api/comment/reply', commentRequest).pipe(map(data => { return <CommentDetails>data}));
  }

  public getComments(targetId: string): Observable<CommentDetails[]> {
    return this.http.get(`/api/comment/read?targetId=${targetId}`).pipe(map(data => { return <CommentDetails[]>data}));
  }

  public showCommentControl(request: CommenterRequest){
    this.onShowCommentControl.emit(request);
  }
}
