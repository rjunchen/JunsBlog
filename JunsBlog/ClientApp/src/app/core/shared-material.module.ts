import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";


@NgModule({
  declarations: [  ],
  imports: [
    CommonModule, MatButtonModule, MatCheckboxModule, FormsModule, ReactiveFormsModule
  ],
  exports:[ MatButtonModule, MatCheckboxModule, FormsModule , ReactiveFormsModule ]
})
export class SharedMaterialModule { }