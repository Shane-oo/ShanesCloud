import {Component, input, output} from '@angular/core';
import {FolderModel, PageRouteLinkModel} from "./left-sidebar.models";

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.component.html',
  styleUrl: './left-sidebar.component.css'
})
export class LeftSidebarComponent {
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

  public folders: FolderModel[] = [{
    name: 'testFolder1',
    folders: [{
      name: 'childTestFolder2',
      folders: []
    }]
  }];

  public toggleCollapse(): void {
    this.changeIsLeftSidebarCollapsed.emit(!this.isLeftSidebarCollapsed());
  }

  public closeSidenav(): void {
    this.changeIsLeftSidebarCollapsed.emit(true);
  }
}
