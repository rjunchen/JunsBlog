import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ResetRoutingModule } from './reset-routing.module';
import { ResetComponent } from './reset.component';
import { SharedMaterialModule } from 'src/app/core/shared-material.module';
import { MatStepperModule } from '@angular/material/stepper'

@NgModule({
  declarations: [ResetComponent],
  imports: [
    CommonModule,
    ResetRoutingModule, SharedMaterialModule, MatStepperModule
  ]
})
export class ResetModule { }
