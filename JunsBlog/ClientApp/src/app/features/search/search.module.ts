import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SearchRoutingModule } from './search-routing.module';
import { SearchComponent } from './search.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { ArticlesListModule } from '../articles-list/articles-list.modules';
import { MatButtonToggleModule } from '@angular/material/button-toggle'

@NgModule({
  declarations: [SearchComponent],
  imports: [
    CommonModule,
    SearchRoutingModule, SharedMaterialModule, ArticlesListModule, MatButtonToggleModule
  ]
})
export class SearchModule { }
