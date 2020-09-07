import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { MatDividerModule } from '@angular/material/divider';


@NgModule({
  declarations: [LoginComponent],
  imports: [
    CommonModule,
    LoginRoutingModule, SharedMaterialModule, MatDividerModule
  ]
})
export class LoginModule { }
