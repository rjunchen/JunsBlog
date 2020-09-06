import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticleViewerRoutingModule } from './article-viewer-routing.module';
import { ArticleViewerComponent } from './article-viewer.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { QuillModule } from 'ngx-quill';
import { CommentComponent } from './comment/comment.component';
import { CommentDisplayerComponent } from './comment/comment-displayer/comment-displayer.component';
import { CommentControlComponent } from './comment/comment-control/comment-control.component';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { TimeAgoModule } from 'src/app/services/time-ago/time-ago.module';

@NgModule({
  declarations: [ArticleViewerComponent, CommentComponent, CommentDisplayerComponent, CommentControlComponent],
  imports: [
    CommonModule,
    ArticleViewerRoutingModule, SharedMaterialModule, InfiniteScrollModule, TimeAgoModule, 
    QuillModule.forRoot()
  ]
})
export class ArticleViewerModule { }
