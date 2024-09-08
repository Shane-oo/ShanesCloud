import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';

import {UserLoginComponent} from './user-login/user-login.component';
import {UserDetailsComponent} from './user-details/user-details.component';
import {RouterModule} from "@angular/router";
import {USERS_ROUTES} from "./users.routes";
import {FormsModule, ReactiveFormsModule} from "@angular/forms";


@NgModule({
  declarations: [
    UserLoginComponent,
    UserDetailsComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild(USERS_ROUTES),
    FormsModule,
    ReactiveFormsModule
  ]
})
export class UsersModule {
}
