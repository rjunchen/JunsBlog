import { TestBed, async } from '@angular/core/testing';

import { CommentService } from './comment.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CommentRequest } from '../models/comment/commentRequest';
import { CommentRankingRequest } from '../models/comment/commentRankingRequest';
import { RankEnum } from '../models/enums/rankEnum';
import { CommentRankingDetails } from '../models/comment/commentRankingDetails';
import { CommentSearchPagingOption } from '../models/comment/commentSearchPagingOption';
import { CommentSearchPagingResult } from '../models/comment/CommentSearchPagingResult';
import { CommentDetails } from '../models/comment/commentDetails';
import { ArticleDetails } from '../models/article/articleDetails';

describe('CommentService', () => {
  let service: CommentService;
  let httpMock: HttpTestingController;
  
  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule]
    });

    httpMock = TestBed.inject(HttpTestingController);
    service = TestBed.inject(CommentService);
  });

  afterEach(() => {
    httpMock.verify();
  });  

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('rankComment() should return data', async(() => {
    let commentId = '1234567';
    let testUrl = `/api/comment/rank`;
    let data: CommentRankingRequest = new CommentRankingRequest(commentId, RankEnum.Like);
    let testData: CommentRankingDetails = new CommentRankingDetails();

    expect(service).toBeTruthy();
    service.rankComment(data).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(CommentRankingDetails);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('POST');
    req.flush(testData);
  }));

  it('searchComments() should return data', async(() => {
    let commentId = '1234567';
    let testUrl = `/api/comment/search`;
    let data: CommentSearchPagingOption = new CommentSearchPagingOption();
    let testData: CommentSearchPagingResult = new CommentSearchPagingResult();

    expect(service).toBeTruthy();
    service.searchComments(data).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(CommentSearchPagingResult);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('POST');
    req.flush(testData);
  }));

  it('replyArticle() should return data', async(() => {
    let commentId = '1234567';
    let testUrl = `/api/comment/reply`;
    let articleDetail: ArticleDetails = new ArticleDetails();
    articleDetail.id = '1234';
    let data: CommentRequest = new CommentRequest(articleDetail);
    let testData: CommentDetails = new CommentDetails();

    expect(service).toBeTruthy();
    service.replyArticle(data).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(CommentDetails);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('POST');
    req.flush(testData);
  }));

  it('should emit comment on click', () => {
    let comment: CommentDetails = new CommentDetails();
     service.onCommentPosted.subscribe(x=>{
       expect(x).toEqual(comment);
       expect(x).toBeInstanceOf(CommentDetails);
     });

     service.commentPosted(comment);
  });

  it('should emit comment control on click', () => {
    let articleDetail: ArticleDetails = new ArticleDetails();
    articleDetail.id = '1234';
    let comment: CommentRequest = new CommentRequest(articleDetail);
     service.onShowCommentControl.subscribe(x=>{
       expect(x).toEqual(comment);
       expect(x).toBeInstanceOf(CommentRequest);
     });

     service.showCommentControl(comment);
  });

});
