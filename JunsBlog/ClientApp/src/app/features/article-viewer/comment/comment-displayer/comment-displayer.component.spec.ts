import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentDisplayerComponent } from './comment-displayer.component';

describe('CommentDisplayerComponent', () => {
  let component: CommentDisplayerComponent;
  let fixture: ComponentFixture<CommentDisplayerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CommentDisplayerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CommentDisplayerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
