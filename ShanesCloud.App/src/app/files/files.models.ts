import {FormControl} from "@angular/forms";

export interface FolderModel {
  id: number;
  name: string;
  folders: FolderModel[];
  files: FileModel[];
}

export interface FileModel {
  id: number;
  name: string;
}

export interface SearchFolderForm {
  name: FormControl<string| null>;
}
