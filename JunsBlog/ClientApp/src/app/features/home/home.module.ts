import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { HomeComponent } from './home.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { ArticlesListModule } from '../../features/articles-list/articles-list.modules'

@NgModule({
  declarations: [HomeComponent],
  imports: [
    CommonModule,
    HomeRoutingModule, SharedMaterialModule, InfiniteScrollModule, ArticlesListModule
  ]
})
export class HomeModule { }
