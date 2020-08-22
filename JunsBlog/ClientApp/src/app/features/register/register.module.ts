import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RegisterRoutingModule } from './register-routing.module';
import { RegisterComponent } from './register.component';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [RegisterComponent],
  imports: [
    CommonModule,
    RegisterRoutingModule,
    FormsModule, ReactiveFormsModule, 
    MatDividerModule, MatIconModule, MatInputModule, MatFormFieldModule, MatButtonModule
  ]
})
export class RegisterModule { }
