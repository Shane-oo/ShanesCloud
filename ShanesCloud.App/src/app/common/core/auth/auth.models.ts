export interface AuthenticationModel {
  access_token: string;
  id_token: string;
  expires_in: number;
  refresh_token: string;
  scope: string;
  token_type: string;
  username: string;
  role: string;
}

export enum Roles {
  None = "None",
  Admin = "Admin",
  User = "User",
  Guest = "Guest"
}
