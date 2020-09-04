import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatIconModule } from "@angular/material/icon";
import { MatInputModule } from "@angular/material/input";
import { MatFormFieldModule } from "@angular/material/form-field";
import { MatProgressSpinnerModule } from "@angular/material/progress-spinner";
import { FlexLayoutModule } from '@angular/flex-layout';

@NgModule({
  declarations: [  ],
  imports: [
    CommonModule, MatButtonModule, MatCheckboxModule, FormsModule, ReactiveFormsModule, MatIconModule,
    MatInputModule, FlexLayoutModule, MatProgressSpinnerModule
  ],
  exports:[ MatButtonModule, MatCheckboxModule, FormsModule , ReactiveFormsModule, MatIconModule,
    MatInputModule, MatFormFieldModule, FlexLayoutModule, MatProgressSpinnerModule ]
})
export class SharedMaterialModule { }