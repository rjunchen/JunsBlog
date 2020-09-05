import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticleViewerRoutingModule } from './article-viewer-routing.module';
import { ArticleViewerComponent } from './article-viewer.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { QuillModule } from 'ngx-quill';

@NgModule({
  declarations: [ArticleViewerComponent],
  imports: [
    CommonModule,
    ArticleViewerRoutingModule, SharedMaterialModule,
    QuillModule.forRoot()
  ]
})
export class ArticleViewerModule { }
