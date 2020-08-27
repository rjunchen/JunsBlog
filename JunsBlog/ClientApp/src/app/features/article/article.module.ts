import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticleRoutingModule } from './article-routing.module';
import { ArticleComponent } from './article.component';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { QuillModule } from 'ngx-quill';
import { CommentsComponent } from './comments/comments.component';
import { CommentControlComponent } from './comment-control/comment-control.component';
import { CommentDisplayerComponent } from './comment-displayer/comment-displayer.component';
import { TimeAgoModule } from 'src/app/services/time-ago/time-ago.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';

@NgModule({
  declarations: [ArticleComponent, CommentsComponent, CommentControlComponent, CommentDisplayerComponent],
  imports: [
    CommonModule,
    ArticleRoutingModule,
    FormsModule, ReactiveFormsModule, 
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatProgressSpinnerModule,
    MatTooltipModule, TimeAgoModule, InfiniteScrollModule,
    QuillModule.forRoot()
  ]
})
export class ArticleModule { }
