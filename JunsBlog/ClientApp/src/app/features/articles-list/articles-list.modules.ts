import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticlesListComponent } from './articles-list.component';
import { RouterModule } from '@angular/router';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { TimeAgoModule } from '../../services/time-ago/time-ago.module';
import { MatChipsModule } from '@angular/material/chips';
import { NgxGalleryModule } from 'ngx-gallery-9';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';

@NgModule({
  declarations: [ArticlesListComponent],
  imports: [
    CommonModule,
    RouterModule,
    InfiniteScrollModule, TimeAgoModule, NgxGalleryModule, MatChipsModule, SharedMaterialModule
  ],
  exports:[ArticlesListComponent]
})
export class ArticlesListModule { }