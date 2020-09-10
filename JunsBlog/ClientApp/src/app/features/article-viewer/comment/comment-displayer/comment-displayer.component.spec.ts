import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentDisplayerComponent } from './comment-displayer.component';
import { AppModule } from 'src/app/app.module';
import { ArticleViewerModule } from '../../article-viewer.module';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { CommentDetails } from 'src/app/models/comment/commentDetails';
import { User } from 'src/app/models/authentication/user';
import { CommentRankingDetails } from 'src/app/models/comment/commentRankingDetails';

describe('CommentDisplayerComponent', () => {
  let component: CommentDisplayerComponent;
  let fixture: ComponentFixture<CommentDisplayerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentDisplayerComponent ],
      imports:[AppModule, ArticleViewerModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentDisplayerComponent);
    component = fixture.componentInstance;
    
    let user = new User();
    user.createdOn = new Date().toDateString();
    user.email = 'Test@test.com',
    user.id = '1234567890',
    user.image = './assets/avatar.png',
    user.name = 'tester',
    user.role = 'user',
    user.type = 'local',
    user.updatedOn = new Date().toDateString();

    let articleDetails = new ArticleDetails();
    articleDetails.id = '1234567';
    articleDetails.author = user;

    let commentDetails = new CommentDetails();
    commentDetails.id = '12345678';

    let commentRanking = new CommentRankingDetails();
    commentRanking.didIDislike = true;
    commentRanking.didIFavor = false;
    commentRanking.didILike = false;
    commentRanking.dislikesCount = 0;
    commentRanking.likesCount = 4;

    commentDetails.user = user;
    commentDetails.articleId = articleDetails.id;
    commentDetails.childrenCommentsCount = 0;
    commentDetails.commentText = 'This is a comment';
    commentDetails.parentId = null;
    commentDetails.ranking = commentRanking;
    commentDetails.updatedOn = new Date().toDateString();

    component.comment = commentDetails;
    component.article = articleDetails;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
