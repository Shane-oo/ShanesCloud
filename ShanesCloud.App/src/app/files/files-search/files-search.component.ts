import { Component } from '@angular/core';
import {FormControl, FormGroup} from "@angular/forms";

import {SearchFolderForm} from "../files.models";

@Component({
  selector: 'app-files-search',
  templateUrl: './files-search.component.html',
  styleUrl: './files-search.component.css'
})
export class FilesSearchComponent {
  public searchFolderForm: FormGroup<SearchFolderForm>;
  private searchValue: string = '';

  constructor() {
    this.searchFolderForm = new FormGroup<SearchFolderForm>({
      name: new FormControl()
    });
  }

  public onSearchSubmit(): void {
    this.searchValue = this.searchFolderForm.value.name ?? '';

    console.log('fetch using', this.searchValue);
  }
}
