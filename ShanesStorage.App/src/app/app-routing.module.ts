import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {MainComponent} from "./main/main.component";
import {FooComponent} from "./foo/foo.component";

const routes: Routes = [
  {path: '', component: MainComponent, pathMatch: 'full'},
  {path:'foo', component: FooComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
