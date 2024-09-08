import {Component, signal} from '@angular/core';

import {FolderModel} from "./files.models";

@Component({
  selector: 'app-files',
  templateUrl: './files.component.html',
  styleUrl: './files.component.css'
})
export class FilesComponent {
  public folders = signal<FolderModel[]>([]);

  constructor() {


    // simulate getting computers (first folders)
    this.folders.set( [
      {
        id: 1,
        name: 'Home Computer',
        folders: [
          {
            id: 2,
            name: 'Documents',
            folders: [
              {
                id: 3,
                name: 'Models',
                folders: [],
                files: [],
              }
            ],
            files: [
              {
                id: 4,
                name: 'MyFile.txt'
              }
            ]
          },
          {
            id: 5,
            name: 'Downloads',
            folders: [],
            files: [
              {
                id: 6,
                name: 'MyDownload1.text'
              },
              {
                id: 7,
                name: 'MyDownload2.text'
              }
            ]
          }
        ],
        files: [{
          id: 8,
          name: 'appsettings.json'
        },
        ]
      },
      {
        id: 10,
        name: 'Macbook',
        files: [
          {
            id: 99,
            name: 'random.txt'
          }
        ],
        folders: []
      }
    ]);
  }


}
