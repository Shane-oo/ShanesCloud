import {Component, computed, input} from '@angular/core';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent {
  public isLeftSidebarCollapsed = input.required<boolean>();
  public screenWidth = input.required<number>();
  public sizeClass = computed(() => {
    if (this.isLeftSidebarCollapsed()) {
      return '';
    }
    return this.screenWidth() > 765 ? 'body-md-screen' : 'body-trimmed';
  });

}
