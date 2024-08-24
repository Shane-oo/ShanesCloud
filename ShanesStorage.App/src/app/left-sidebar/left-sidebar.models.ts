export interface PageRouteLinkModel {
  routeLink: string;
  label: string;
  icon: string;
}

export interface FolderModel {
  name: string;
  folders: FolderModel[];
}
