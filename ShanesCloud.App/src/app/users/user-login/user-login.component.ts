import {Component, DestroyRef, inject} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";

import {AuthService} from "../../common/core/auth/auth.service";
import {AuthenticationModel} from "../../common/core/auth/auth.models";

import {UserLoginForm, UserLoginRequest} from "../users.models";

@Component({
  selector: 'app-user-login',
  templateUrl: './user-login.component.html',
  styleUrl: './user-login.component.css'
})
export class UserLoginComponent {
  private readonly destroyRef = inject(DestroyRef)
  public loginForm: FormGroup<UserLoginForm>;

  constructor(private readonly authService: AuthService) {
    this.loginForm = new FormGroup<UserLoginForm>({
      email: new FormControl(null, [Validators.required, Validators.email]),
      password: new FormControl(null, Validators.required)
    });
  }

  logIn() {
    if (this.loginForm.valid) {
      const loginFormValue = this.loginForm.value;

      const loginRequest: UserLoginRequest = {
        email: loginFormValue.email || "",
        password: loginFormValue.password || ""
      };

      this.authService.logIn(loginRequest)
        .pipe(takeUntilDestroyed(this.destroyRef))
        .subscribe((response: AuthenticationModel) => {
        });
    }
  }
}
