import {Component, computed, input, OnInit, output, signal} from '@angular/core';

import {PageRouteLinkModel} from "./left-sidebar.models";
import {AuthService} from "../common/core/auth/auth.service";
import {StateService} from "../common/core/state/state.service";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";

@Component({
  selector: 'app-left-sidebar',
  templateUrl: './left-sidebar.component.html',
  styleUrl: './left-sidebar.component.css'
})
export class LeftSidebarComponent implements OnInit {
  public isUserAuthenticated = signal<boolean>(false);
  public isUserAdmin = signal<boolean>(false);
  public userName = computed(() => {
    if (!this.isUserAuthenticated) {
      return '';
    }
    return this.stateService.userName;
  });
  public isLeftSidebarCollapsed = input.required<boolean>();
  public changeIsLeftSidebarCollapsed = output<boolean>();

  public itemsWhenLoggedIn: PageRouteLinkModel[] = [
    {
      routeLink: '',
      label: 'Home',
      icon: 'lnr lnr-home'
    },
    // todo files
    {
      routeLink: 'users/details',
      label: this.userName(),
      icon: 'lnr lnr-user'
    }
  ];

  public itemsWhenNotLoggedIn: PageRouteLinkModel[] = [
    {
      routeLink: 'users/login',
      label: 'Login',
      icon: 'lnr lnr-user'
    }
  ];


  constructor(private authService: AuthService,
              private stateService: StateService) {
    this.authService.authChanged
      .pipe(takeUntilDestroyed())
      .subscribe((isAuthenticated: boolean) => {
        this.isUserAuthenticated.set(isAuthenticated);
        this.isUserAdmin.set(this.authService.isUserAdmin());
      });
  }

  public ngOnInit() {

  }

  public toggleCollapse(): void {
    this.changeIsLeftSidebarCollapsed.emit(!this.isLeftSidebarCollapsed());
  }

  public closeSidenav(): void {
    this.changeIsLeftSidebarCollapsed.emit(true);
  }
}
