import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeftSidebarComponent } from './left-sidebar.component';
import {RouterLink, RouterLinkActive} from "@angular/router";



@NgModule({
  declarations: [
    LeftSidebarComponent
  ],
  exports: [
    LeftSidebarComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
  ]
})
export class LeftSidebarModule { }
