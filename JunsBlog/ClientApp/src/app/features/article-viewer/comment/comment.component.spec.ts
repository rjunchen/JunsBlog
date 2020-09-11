import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentComponent } from './comment.component';
import { AppModule } from 'src/app/app.module';
import { ArticleViewerModule } from '../article-viewer.module';
import { Article } from 'src/app/models/article/article';
import { ArticleDetails } from 'src/app/models/article/articleDetails';

describe('CommentComponent', () => {
  let component: CommentComponent;
  let fixture: ComponentFixture<CommentComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentComponent ],
      imports:[AppModule, ArticleViewerModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentComponent);
    component = fixture.componentInstance;

    let articleDetails = new ArticleDetails();
    articleDetails.id = '1234567';

    component.article = articleDetails;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
