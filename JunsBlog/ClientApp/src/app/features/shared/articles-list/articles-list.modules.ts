import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ArticlesListComponent } from './articles-list.component';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { RouterModule } from '@angular/router';
import { InfiniteScrollModule } from 'ngx-infinite-scroll';
import { TimeAgoModule } from 'src/app/services/time-ago/time-ago.module';
import { MatChipsModule } from '@angular/material/chips';
import { NgxGalleryModule } from 'ngx-gallery-9';

@NgModule({
  declarations: [ArticlesListComponent],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule, ReactiveFormsModule, 
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatProgressSpinnerModule,
    InfiniteScrollModule, TimeAgoModule, MatChipsModule, NgxGalleryModule
  ],
  exports:[ArticlesListComponent]
})
export class ArticlesListModule { }