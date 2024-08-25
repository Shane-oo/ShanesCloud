import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LeftSidebarComponent } from './left-sidebar.component';
import {RouterLink, RouterLinkActive} from "@angular/router";
import {ReactiveFormsModule} from "@angular/forms";



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
    ReactiveFormsModule
  ]
})
export class LeftSidebarModule { }
