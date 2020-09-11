import { TestBed, async } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing'
import { ArticleService } from './article.service';
import { HttpClientModule } from '@angular/common/http';
import { Article } from '../models/article/article';
import { ArticleDetails } from '../models/article/articleDetails';
import { ArticleRankingDetails } from '../models/article/articleRankingDetails';
import { RankEnum } from '../models/enums/rankEnum';
import { ArticleSearchPagingOption } from '../models/article/articleSearchPagingOption';

describe('ArticleService', () => {
  let service: ArticleService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientModule, HttpClientTestingModule]
    });

    httpMock = TestBed.inject(HttpTestingController);
    service = TestBed.inject(ArticleService);
  });

  afterEach(() => {
    httpMock.verify();
  });  

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getArticle() should return data', async(() => {
    let articleId = '123456';
    let testUrl = `/api/article/get?articleId=${articleId}`;
    let testData: Article = new Article();

    expect(service).toBeTruthy();
    service.getArticle(articleId).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(Article);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('GET');
    req.flush(testData);
  }));


  it('getArticleDetails() should return data', async(() => {
    let articleId = '123456';
    let testUrl = `/api/article/details/get?articleId=${articleId}`;
    let testData: ArticleDetails = new ArticleDetails();

    expect(service).toBeTruthy();
    service.getArticleDetails(articleId).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(ArticleDetails);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('GET');
    req.flush(testData);
  }));

  it('getArticleRankingDetails() should return data', async(() => {
    let articleId = '123456';
    let testUrl = `/api/article/rank?articleId=${articleId}`;
    let testData: ArticleRankingDetails = new ArticleRankingDetails();

    expect(service).toBeTruthy();
    service.getArticleRankingDetails(articleId).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(ArticleRankingDetails);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('GET');
    req.flush(testData);
  }));

  it('saveArticle() should return data', async(() => {
    let articleId = '123456';
    let testUrl = `/api/article/save`;
    let article: Article = new Article();
    article.id = articleId;
    let testData = { id: articleId }

    expect(service).toBeTruthy();
    service.saveArticle(article).subscribe( data=> {
      expect(data).toEqual(testData);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('POST');
    req.flush(testData);
  }));

  it('rankArticle() should return data', async(() => {
    let articleId = '123456';
    let testUrl = `/api/article/rank`;
    let article: Article = new Article();
    article.id = articleId;
    let testData: ArticleRankingDetails = new ArticleRankingDetails();

    expect(service).toBeTruthy();
    service.rankArticle(articleId, RankEnum.Like).subscribe( data=> {
      expect(data).toEqual(testData);
      expect(data).toBeInstanceOf(ArticleRankingDetails);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('POST');
    req.flush(testData);
  }));

  it('searchArticle() should return data', async(() => {
    let articleId = '123456';
    let testUrl = `/api/article/search`;
    let searchOption: ArticleSearchPagingOption = new ArticleSearchPagingOption();
    let testData: ArticleRankingDetails = new ArticleRankingDetails();

    expect(service).toBeTruthy();
    service.searchArticle(searchOption).subscribe( data=> {
      expect(data).toBeInstanceOf(ArticleRankingDetails);
    })

    const req = httpMock.expectOne(testUrl);
    expect(req.request.method).toEqual('POST');
    req.flush(testData);
  }));

  it('should emit on click', () => {
    let searchOption: ArticleSearchPagingOption = new ArticleSearchPagingOption();
     service.onSearchClicked.subscribe(x=>{
       expect(x).toEqual(searchOption);
       expect(x).toBeInstanceOf(ArticleSearchPagingOption);
     });

     service.SearchClicked(searchOption);
  });

});
