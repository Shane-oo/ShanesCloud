import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {RouterLink, RouterLinkActive} from "@angular/router";

import { LeftSidebarComponent } from './left-sidebar.component';

import {FilesModule} from "../files/files.module";



@NgModule({
  declarations: [
    LeftSidebarComponent,
  ],
  exports: [
    LeftSidebarComponent
  ],
  imports: [
    CommonModule,
    RouterLink,
    RouterLinkActive,
    FilesModule
  ]
})
export class LeftSidebarModule { }
