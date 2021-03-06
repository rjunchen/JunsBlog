import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HeaderComponent } from './header/header.component'
import { SidebarLeftComponent } from './sidebar-left/sidebar-left.component'
import { SharedMaterialModule } from './shared-material.module'
import { MatToolbarModule } from '@angular/material/toolbar'; 
import { MatSidenavModule } from '@angular/material/sidenav';
import { RouterModule } from '@angular/router';


@NgModule({
  declarations: [ HeaderComponent, SidebarLeftComponent ],
  imports: [
    CommonModule, SharedMaterialModule, MatToolbarModule, MatSidenavModule, RouterModule
  ],
  exports:[HeaderComponent, SidebarLeftComponent, MatToolbarModule, MatSidenavModule, RouterModule]
})
export class CoreModule { }
