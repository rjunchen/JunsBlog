import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ProfileRoutingModule } from './profile-routing.module';
import { ProfileComponent } from './profile.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { ProfileEditorComponent } from './profile-editor/profile-editor.component';
import { ImageCropperModule } from 'ngx-image-cropper';
import { ArticlesListModule } from '../articles-list/articles-list.modules';

@NgModule({
  declarations: [ProfileComponent, ProfileEditorComponent],
  imports: [
    CommonModule,
    ProfileRoutingModule, SharedMaterialModule, ImageCropperModule, ArticlesListModule
  ]
})
export class ProfileModule { }
