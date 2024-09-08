import {Injectable} from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class StateService {
  private readonly returnUrlKey = 'ShanesCloud.RedirectUrl';
  private readonly userNameKey = 'ShanesCloud.UserName';

  constructor() {
  }

  get returnUrl(): string {
    return localStorage.getItem(this.returnUrlKey) ?? "/";
  }

  set returnUrl(value: string) {
    localStorage.setItem(this.returnUrlKey, value);
  }

  get userName(): string {
    return localStorage.getItem(this.userNameKey) ?? "";
  }

  set userName(value: string) {
    localStorage.setItem(this.userNameKey, value);
  }
}
