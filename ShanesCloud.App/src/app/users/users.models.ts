import {FormControl} from "@angular/forms";

export interface UserLoginForm {
  email: FormControl<string | null>;
  password: FormControl<string | null>;
}

export interface UserLoginRequest {
  email: string;
  password: string;
}
