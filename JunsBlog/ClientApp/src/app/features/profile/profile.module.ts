import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile.component';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { ArticlesListModule } from '../shared/articles-list/articles-list.modules';
import { ProfileEditorComponent } from './profile-editor/profile-editor.component';
import { ImageCropperModule } from 'ngx-image-cropper';


@NgModule({
  declarations: [ProfileComponent, ProfileEditorComponent],
  imports: [
    CommonModule,
    ProfileRoutingModule, ImageCropperModule,
    FormsModule, ReactiveFormsModule, 
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule, MatProgressSpinnerModule,
    MatTooltipModule, ArticlesListModule
  ]
})
export class ProfileModule { }
