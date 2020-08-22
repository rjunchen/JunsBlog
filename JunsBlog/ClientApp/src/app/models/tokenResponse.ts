import { User } from './user';

export class TokenResponse {
  refreshToken: string;
  accessToken: string;
  user: User;
}