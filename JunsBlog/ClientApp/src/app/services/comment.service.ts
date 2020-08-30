import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CommentDetails } from '../models/comment/commentDetails';
import { Observable } from 'rxjs';
import { SortOrderEnum } from '../models/Enums/sortOrderEnum';
import { SortByEnum } from '../models/Enums/sortByEnum';
import { commentSearchOnEnum } from '../models/Enums/commentSearchOnEnum';
import { CommentRequest } from '../models/comment/commentRequest';
import { CommentSearchPagingResult } from '../models/comment/commentSearchPagingResult';
import { CommentRankingRequest } from '../models/comment/commentRankingRequest';
import { CommentRankingDetails } from '../models/comment/commentRankingDetails';
import { CommentSearchPagingOption } from '../models/comment/commentSearchPaingOption';


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

  public searchComments(option: CommentSearchPagingOption) : Observable<CommentSearchPagingResult> {
    return this.http.post('/api/comment/search', option).pipe(map(data => { return <CommentSearchPagingResult>data}));
  }

  public showCommentControl(request: CommentRequest){
    this.onShowCommentControl.emit(request);
  }

  public commentPosted(comment: CommentDetails){
    this.onCommentPosted.emit(comment);
  }

  public rankComment(rankRequest: CommentRankingRequest): Observable<CommentRankingDetails>{
    return this.http.post('/api/comment/rank', rankRequest).pipe(map(data => { return <CommentRankingDetails>data}));
  }
}
