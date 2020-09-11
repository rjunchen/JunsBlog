import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentControlComponent } from './comment-control.component';
import { AppModule } from 'src/app/app.module';
import { ArticleViewerModule } from '../../article-viewer.module';

describe('CommentControlComponent', () => {
  let component: CommentControlComponent;
  let fixture: ComponentFixture<CommentControlComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentControlComponent ],
      imports:[AppModule, ArticleViewerModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
