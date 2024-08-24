import {Component, input, OnInit, output} from '@angular/core';
import {FolderModel, PageRouteLinkModel, SearchFolderForm} from "./left-sidebar.models";
import {FormControl, FormGroup} from "@angular/forms";

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.component.html',
  styleUrl: './left-sidebar.component.css'
})
export class LeftSidebarComponent implements OnInit {
  public isLeftSidebarCollapsed = input.required<boolean>();
  public changeIsLeftSidebarCollapsed = output<boolean>();
  public items: PageRouteLinkModel[] = [
    {
      routeLink: '',
      label: 'Home',
      icon: 'lnr lnr-home'
    },
    {
      routeLink: 'foo',
      label: 'Foo',
      icon: 'lnr lnr-rocket'
    }
  ];

  public folders: FolderModel[] = [];
  public searchFolderForm: FormGroup<SearchFolderForm>;
  private searchValue: string = '';

  constructor() {

    this.searchFolderForm = new FormGroup<SearchFolderForm>({
      name: new FormControl('', {
        nonNullable: true
      })
    });

  }

  public ngOnInit() {
    // simulate getting computers (first folders)
    this.folders = [
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
    ];

  }

  public toggleCollapse(): void {
    this.changeIsLeftSidebarCollapsed.emit(!this.isLeftSidebarCollapsed());
  }

  public closeSidenav(): void {
    this.changeIsLeftSidebarCollapsed.emit(true);
  }

  public onSearchSubmit(): void {
    this.searchValue = this.searchFolderForm.value.name ?? '';

    console.log('fetch using', this.searchValue);
  }
}
