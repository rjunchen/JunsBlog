import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ArticleEditorComponent } from './article-editor.component';

const routes: Routes = [{ path: '', component: ArticleEditorComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ArticleEditorRoutingModule { }
