import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginRoutingModule } from './login-routing.module';
import { LoginComponent } from './login.component';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [LoginComponent],
  imports: [
    FormsModule, ReactiveFormsModule, 
    CommonModule,
    LoginRoutingModule,
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule
  ]
})
export class LoginModule { }
