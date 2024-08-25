import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import {LeftSidebarModule} from "./left-sidebar/left-sidebar.module";
import { FooComponent } from './foo/foo.component';

@NgModule({
  declarations: [
    AppComponent,
    FooComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LeftSidebarModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
