import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { FilesComponent } from './files.component';
import { FilesSearchComponent } from './files-search/files-search.component';
import {ReactiveFormsModule} from "@angular/forms";
import {HttpClientModule} from "@angular/common/http";



@NgModule({
  declarations: [
    FilesComponent,
    FilesSearchComponent
  ],
  exports: [
    FilesComponent
  ],
  imports: [
    CommonModule,
    ReactiveFormsModule
  ]
})
export class FilesModule { }
