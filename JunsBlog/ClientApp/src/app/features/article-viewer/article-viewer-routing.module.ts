import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { ArticleViewerComponent } from './article-viewer.component';

const routes: Routes = [{ path: '', component: ArticleViewerComponent }];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ArticleViewerRoutingModule { }
