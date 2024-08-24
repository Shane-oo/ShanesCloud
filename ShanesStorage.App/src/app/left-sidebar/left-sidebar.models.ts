import {FormControl} from "@angular/forms";

export interface PageRouteLinkModel {
  routeLink: string;
  label: string;
  icon: string;
}

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
  name: FormControl<string>;
}
