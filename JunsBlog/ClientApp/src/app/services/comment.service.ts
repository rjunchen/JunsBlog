import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { CommentDetails } from '../models/comment/commentDetails'
import { CommentRequest } from '../models/comment/commentRequest'

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  @Output() onShowCommentControl: EventEmitter<any> = new EventEmitter();
  @Output() onCommentPosted: EventEmitter<CommentDetails> = new EventEmitter();
  constructor(private http: HttpClient) { }

  public replyArticle(commentRequest: CommentRequest): Observable<CommentDetails> {
    return this.http.post('/api/comment/reply', commentRequest).pipe(map(data => { return <CommentDetails>data}));
  }

  public showCommentControl(request: CommentRequest){
    this.onShowCommentControl.emit(request);
  }

  public commentPosted(comment: CommentDetails){
    this.onCommentPosted.emit(comment);
  }
}
