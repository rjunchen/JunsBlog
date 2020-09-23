import { Injectable, Output, EventEmitter } from '@angular/core';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { AuthResponse } from './../models/authentication/authResponse'
import { User } from '../models/authentication/user';
import { Router } from '@angular/router';
import { Profile } from './../models/authentication/profile'

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  @Output() onUserInfoUpdated: EventEmitter<User> = new EventEmitter();
  private token: string;
  private currentUser: User;
  private googleAuthUrl: string;
  private defaultAvatarUrl = './assets/avatar.png';

  constructor(private http: HttpClient, private router: Router) {
    try{
      this.token = localStorage.getItem('accessToken');
      this.currentUser = JSON.parse(localStorage.getItem('user'));
    }catch(err){
      this.logout();
    }
   }

  public register(email: string, password: string, name: string): Observable<boolean>{
    return this.http.post<AuthResponse>('/api/user/register', {email, password, name, image: this.defaultAvatarUrl }).pipe(map(data=>{
      if(data){
        this.saveToken(data);
      }
      return true;
    }))
  }

  public login(formData): Observable<boolean> {
    return this.http.post<AuthResponse>('/api/user/authenticate', formData).pipe(map(data=>{
      if(data){
        this.saveToken(data);
      }
      return true;
    }))
  }

  public logout(){
    this.currentUser = null;
    this.token = null;
    window.localStorage.removeItem('accessToken');
    window.localStorage.removeItem('user');
    this.onUserInfoUpdated.emit(null);
    this.router.navigateByUrl('/login');
  }

  public isLoggedIn(): boolean{
    if(this.currentUser){
      return true;
    }else{
      return false;
    }
  }

  public getAuthenticationInfo(accessToken: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>('/api/user/auth/info',{accessToken}).pipe(map(data => {
      if(data){
        this.saveToken(data);
      }
      return data;
     }));
  }

  public getGoogleAuthUrl(): Observable<string> {
    if(this.googleAuthUrl)
      return of(this.googleAuthUrl);
    return this.http.get('/api/user/auth/google/url',{responseType: 'text'}).pipe(map((data: string) => {
       this.googleAuthUrl = data;
       return data;
     }));
  }

  public saveToken(authResponse: AuthResponse){
    if(authResponse){
      this.token = authResponse.accessToken;
      this.currentUser = authResponse.user;
      localStorage.setItem('accessToken', this.token);
      localStorage.setItem('user', JSON.stringify(authResponse.user));
      this.onUserInfoUpdated.emit(authResponse.user);
    }
  }

  getCurrentUser(): User {
    return this.currentUser;
  }

  public getToken(): string{
    return this.token;
  }

  public getProfile(userId: string): Observable<Profile>{
    return this.http.get<Profile>(`/api/user/profile?userId=${userId}`);
  }

  public updateProfile(id: string, name: string, email: string, image: string): Observable<User> {
    return this.http.post<User>(`/api/user/profile/update`, { id, name, email, image }).pipe(map(user  => {
      this.currentUser = user;
      localStorage.setItem('user', JSON.stringify(user));
       this.onUserInfoUpdated.emit(user);
       return user;
      }));
  }

  public sendResetToken(email: string){
    return this.http.get(`/api/user/reset/verifyEmail?email=${email}`);
  }

  public verifyToken(email: string, token: string){
    return this.http.get(`/api/user/reset/verifyToken?email=${email}&token=${token}`);
  }

  public resetPassword(email: string, password: string, resetToken: string ){
    return this.http.post('/api/user/reset/password', {email, password, resetToken});
  }

}
