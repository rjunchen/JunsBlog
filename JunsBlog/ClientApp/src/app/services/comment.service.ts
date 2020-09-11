import { Injectable, Output, EventEmitter } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { CommentDetails } from '../models/comment/commentDetails'
import { CommentRequest } from '../models/comment/commentRequest'
import { CommentSearchPagingOption } from '../models/comment/commentSearchPagingOption'
import { CommentSearchPagingResult } from '../models/comment/CommentSearchPagingResult'
import { CommentRankingRequest } from '../models/comment/commentRankingRequest';
import { CommentRankingDetails } from '../models/comment/commentRankingDetails';

@Injectable({
  providedIn: 'root'
})
export class CommentService {
  @Output() onShowCommentControl: EventEmitter<any> = new EventEmitter();
  @Output() onCommentPosted: EventEmitter<CommentDetails> = new EventEmitter();
  constructor(private http: HttpClient) { }

  public replyArticle(commentRequest: CommentRequest): Observable<CommentDetails> {
    return this.http.post<CommentDetails>('/api/comment/reply', commentRequest);
  }

  public showCommentControl(request: CommentRequest){
    this.onShowCommentControl.emit(request);
  }

  public commentPosted(comment: CommentDetails){
    this.onCommentPosted.emit(comment);
  }

  public searchComments(option: CommentSearchPagingOption) : Observable<CommentSearchPagingResult> {
    return this.http.post<CommentSearchPagingResult>('/api/comment/search', option);
  }

  public rankComment(rankRequest: CommentRankingRequest): Observable<CommentRankingDetails>{
    return this.http.post<CommentRankingDetails>('/api/comment/rank', rankRequest);
  }
}
