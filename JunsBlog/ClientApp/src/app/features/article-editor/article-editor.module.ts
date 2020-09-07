import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticleEditorRoutingModule } from './article-editor-routing.module';
import { ArticleEditorComponent } from './article-editor.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { MatChipsModule } from '@angular/material/chips'
import { QuillModule } from 'ngx-quill'

@NgModule({
  declarations: [ArticleEditorComponent],
  imports: [
    CommonModule,
    ArticleEditorRoutingModule, SharedMaterialModule, MatChipsModule,  QuillModule.forRoot()
  ]
})
export class ArticleEditorModule { }
