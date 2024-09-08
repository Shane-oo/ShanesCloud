import {CanActivateFn, Router} from '@angular/router';
import {inject} from "@angular/core";

import {AuthService} from "./auth.service";
import {StateService} from "../state/state.service";

export const authGuard: CanActivateFn = (route, state) => {
  const authService = inject(AuthService);

  if (authService.isUserAuthenticated()) {
    return true;
  }

  const stateService = inject(StateService);
  const router = inject(Router);

  stateService.returnUrl = state.url;

  router.navigate(['/users/login']).then(() => {
    console.log("User must login");
  });
  return false;
};
