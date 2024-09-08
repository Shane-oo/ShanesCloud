import {Routes} from "@angular/router";

import {authGuard} from "../common/core/auth/auth.guard";

import {UserLoginComponent} from "./user-login/user-login.component";
import {UserDetailsComponent} from "./user-details/user-details.component";

export const USERS_ROUTES: Routes = [
  {path: 'login', component: UserLoginComponent},
  {path: 'details', component: UserDetailsComponent, canActivate: [authGuard]}
]
