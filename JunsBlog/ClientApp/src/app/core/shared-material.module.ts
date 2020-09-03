import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';


import { MatButtonModule } from '@angular/material/button';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { MatIconModule } from "@angular/material/icon";


@NgModule({
  declarations: [  ],
  imports: [
    CommonModule, MatButtonModule, MatCheckboxModule, FormsModule, ReactiveFormsModule, MatIconModule
  ],
  exports:[ MatButtonModule, MatCheckboxModule, FormsModule , ReactiveFormsModule, MatIconModule ]
})
export class SharedMaterialModule { }