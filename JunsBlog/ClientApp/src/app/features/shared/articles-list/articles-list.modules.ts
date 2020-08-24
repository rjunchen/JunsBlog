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
import { TimeAgoPipe } from 'src/app/services/time-ago.pipe';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [ArticlesListComponent, TimeAgoPipe],
  imports: [
    CommonModule,
    RouterModule,
    FormsModule, ReactiveFormsModule, 
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatProgressSpinnerModule
  ],
  exports:[ArticlesListComponent]
})
export class ArticlesListModule { }