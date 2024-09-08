import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';

import {authGuard} from "./common/core/auth/auth.guard";

import {UsersModule} from "./users/users.module";

import {HomeComponent} from "./home/home.component";


const routes: Routes = [
  {path: '', component: HomeComponent, pathMatch: 'full', canActivate: [authGuard]},
  {path: 'users', loadChildren: () => UsersModule}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
