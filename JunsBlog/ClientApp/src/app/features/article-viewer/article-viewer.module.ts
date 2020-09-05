import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticleViewerRoutingModule } from './article-viewer-routing.module';
import { ArticleViewerComponent } from './article-viewer.component';


@NgModule({
  declarations: [ArticleViewerComponent],
  imports: [
    CommonModule,
    ArticleViewerRoutingModule
  ]
})
export class ArticleViewerModule { }
