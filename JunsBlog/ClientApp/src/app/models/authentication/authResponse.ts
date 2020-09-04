import { User } from "./user";

export class AuthResponse {
    refreshToken: string;
    accessToken: string;
    user: User;
  }