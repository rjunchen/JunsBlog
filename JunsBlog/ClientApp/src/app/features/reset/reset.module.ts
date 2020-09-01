import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResetRoutingModule } from './reset-routing.module';
import { ResetComponent } from './reset.component';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ArticlesListModule } from '../shared/articles-list/articles-list.modules';
import { MatStepperModule } from '@angular/material/stepper'

@NgModule({
  declarations: [ResetComponent],
  imports: [
    CommonModule,
    ResetRoutingModule,
    FormsModule, ReactiveFormsModule, 
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatProgressSpinnerModule,
    MatTooltipModule, ArticlesListModule,MatStepperModule
  ]
})
export class ResetModule { }
