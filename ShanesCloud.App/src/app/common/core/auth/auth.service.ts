import {Injectable} from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {Router} from "@angular/router";
import {Observable, of, Subject, tap} from "rxjs";

import {jwtDecode} from "jwt-decode";

import {environment} from "../../../../environments/environment";

import {StateService} from "../state/state.service";
import {AuthenticationModel, Roles} from "./auth.models";
import {UserLoginRequest} from "../../../users/users.models";


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly endpoint = environment.API_URL + "/auth";
  private readonly key = "ShanesCloud.Api";
  private readonly _clientId = "ShanesCloud";
  private readonly _authChanged = new Subject<boolean>();

  constructor(private readonly http: HttpClient,
              private readonly stateService: StateService,
              private readonly router: Router) {
  }

  get clientId(): string {
    return this._clientId;
  }

  get authChanged(): Observable<boolean> {
    return this._authChanged.asObservable();
  }

  isUserAuthenticated(): boolean {
    return !!localStorage.getItem(this.key);
  }

  isUserAdmin(): boolean {
    if (this.isUserAuthenticated()) {
      return this.getAuthenticationData()?.role === Roles.Admin;
    }
    return false;
  }

  getAuthenticationData(): AuthenticationModel | null {
    let storedAuthenticationData = localStorage.getItem(this.key);
    if (storedAuthenticationData) {
      return JSON.parse(storedAuthenticationData) as AuthenticationModel;
    }

    this.sendAuthStateChangedNotification(false);

    return null;
  }

  setAuthenticationData(value: AuthenticationModel) {
    localStorage.setItem(this.key, JSON.stringify(value));
  }

  sendAuthStateChangedNotification(isAuthenticated: boolean) {
    this._authChanged.next(isAuthenticated);
  }

  logIn(userLoginRequest: UserLoginRequest): Observable<AuthenticationModel> {
    const data = `grant_type=password` +
      `&username=${encodeURIComponent(userLoginRequest.email)}` +
      `&password=${encodeURIComponent(userLoginRequest.password)}` +
      `&client_id=${this._clientId}`;

    const headers = new HttpHeaders({'Content-Type': 'application/x-www-form-urlencoded'});

    return this.http.post<AuthenticationModel>(this.endpoint + '/token', data, {headers: headers})
      .pipe(tap((response: AuthenticationModel) => {
        const idTokenDecoded = jwtDecode<AuthenticationModel>(response.id_token);
        response.role = idTokenDecoded.role;

        this.setAuthenticationData(response);

        this.stateService.userName = idTokenDecoded.username;

        this.sendAuthStateChangedNotification(true);

        this.router.navigate([this.stateService.returnUrl])
          .then(() => {
            this.stateService.returnUrl = "/";
          });
      }));
  }

  // TODO: Auth Interceptor
  refreshToken(): Observable<AuthenticationModel> {
    const authenticationData = this.getAuthenticationData();

    if (authenticationData) {
      const data = `grant_type=refresh_token` +
        `&refresh_token=${authenticationData?.refresh_token}` +
        `&client_id=${this._clientId}`;
      const headers = new HttpHeaders({'Content-Type': 'application/x-www-form-urlencoded'});

      return this.http.post<AuthenticationModel>(this.endpoint + '/token', data, {headers: headers});
    }
    return of(null as any);
  }

  logOut(): void {
    localStorage.removeItem(this.key);
    this.stateService.userName = "";
    this.sendAuthStateChangedNotification(false);
  }
}
