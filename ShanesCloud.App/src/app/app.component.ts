import {Component, computed, HostListener, inject, OnInit, signal} from '@angular/core';
import {DOCUMENT} from "@angular/common";
import {AuthService} from "./common/core/auth/auth.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public isLeftSidebarCollapsed = signal<boolean>(false);
  private screenWidth = signal<number>(0);
  public sizeClass = computed(() => {
    if (this.isLeftSidebarCollapsed()) {
      return '';
    }
    return this.screenWidth() > 765 ? 'body-md-screen' : 'body-trimmed';
  });

  private readonly document: Document = inject(DOCUMENT);
  private readonly window: WindowProxy | null;
  private movedToSmallScreen = false;

  constructor(private readonly authService: AuthService) {
    this.window = this.document.defaultView;
  }

  @HostListener('window:resize')
  onResize() {
    this.setScreenWidth();
    if (this.isSmallScreen()) {
      this.movedToSmallScreen = true;
      this.isLeftSidebarCollapsed.set(true);
    } else if (this.isBigScreen() && this.movedToSmallScreen) {
      this.movedToSmallScreen = false;

      this.isLeftSidebarCollapsed.set(false);
    }
  }

  ngOnInit() {
    this.setScreenWidth();
    this.isLeftSidebarCollapsed.set(this.isSmallScreen());

    if(this.authService.isUserAuthenticated()) {
      this.authService.sendAuthStateChangedNotification(true);
    }
  }

  public changeIsLeftSidebarCollapsed(isLeftSidebarCollapsed: boolean) {
    this.isLeftSidebarCollapsed.set(isLeftSidebarCollapsed);
  }

  private setScreenWidth() {
    this.screenWidth.set(this.window?.innerWidth ?? 0);
  }

  private isSmallScreen(): boolean {
    return this.screenWidth() <= 768;
  }

  private isBigScreen(): boolean {
    return this.screenWidth() >= 850;
  }
}
