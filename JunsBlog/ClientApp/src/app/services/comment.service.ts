import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { CommentRequest } from '../models/commentRequest';
import { CommentDetails } from '../models/commentDetails';
import { Observable } from 'rxjs';
import { SortOrderEnum } from '../models/Enums/sortOrderEnum';
import { CommentSearchPagingResult } from '../models/commentSearchPagingResult';
import { SortByEnum } from '../models/Enums/sortByEnum';
import { commentSearchOnEnum } from '../models/Enums/commentSearchOnEnum';
import { CommentRankingRequest } from '../models/commentRankingRequest';
import { CommentRankingDetails } from '../models/commentRankingDetails';

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

  public searchComments(page: number, pageSize: number, searchKey: string = "", searchOn: commentSearchOnEnum = commentSearchOnEnum.CommentText,
    sortBy: SortByEnum = SortByEnum.CreatedOn, sortOrder: SortOrderEnum = SortOrderEnum.descending) : Observable<CommentSearchPagingResult> {
    return this.http.get(`/api/comment/search?page=${page}&pageSize=${pageSize}&searchKey=${searchKey}&searchOn=${searchOn}
      &sortOrder=${sortOrder}&sortBy=${sortBy}`).pipe(map(data => { return <CommentSearchPagingResult>data}));
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
