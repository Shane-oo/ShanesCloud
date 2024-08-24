import {Component, HostListener, inject, OnInit, signal} from '@angular/core';
import {DOCUMENT} from "@angular/common";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  public isLeftSidebarCollapsed = signal<boolean>(false);
  public screenWidth = signal<number>(0);

  private readonly document: Document = inject(DOCUMENT);
  private readonly window: WindowProxy | null;
  private movedToSmallScreen: boolean = false;

  constructor() {
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
