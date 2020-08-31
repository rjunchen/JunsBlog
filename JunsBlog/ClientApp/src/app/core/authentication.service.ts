import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TokenResponse } from '../models/TokenResponse';
import { User } from '../models/user';
import { of } from 'rxjs';
import { Profile } from '../models/profile/profile';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  @Output() userInfoUpdated: EventEmitter<any> = new EventEmitter();
  private token: string;
  private currentUser: User;
  private googleAuthUrl: string;

  constructor(private http: HttpClient, private router: Router) { 
    try{
      this.token = localStorage.getItem('accessToken');
      this.currentUser = JSON.parse(localStorage.getItem('user'));
    }catch{
      this.logout();
    }
  }

  public saveToken(tokenResponse: TokenResponse){
    if(tokenResponse){
      this.token = tokenResponse.accessToken;
      this.currentUser = tokenResponse.user;
      localStorage.setItem('accessToken', this.token);
      localStorage.setItem('user', JSON.stringify(tokenResponse.user));
      this.userInfoUpdated.emit(tokenResponse.user);
    }
  }

  public getToken(): string{
    return this.token;
  }

  public getCurrentUser(): User{
    return this.currentUser;
  }

  public register(formData): Observable<TokenResponse> {
    return this.http.post('/api/register', formData).pipe(map((data: TokenResponse)=>{
        if(data){
          this.saveToken(data);
        }
        return data;
    }))
  }

  public login(formData): Observable<TokenResponse> {
    return this.http.post('/api/authenticate', formData).pipe(map((data: TokenResponse)=>{
      if(data){
        this.saveToken(data);
      }
      return data;
    }))
  }

  public isLoggedIn(): boolean{
    if(this.currentUser){
      return true;
    }else{
      return false;
    }
  }

  public logout(){
    this.currentUser = null;
    window.localStorage.removeItem('accessToken');
    window.localStorage.removeItem('user');
    this.userInfoUpdated.emit(null);
    this.router.navigateByUrl('/login');
  }

  public getGoogleAuthUrl(): Observable<string> {
    if(this.googleAuthUrl)
      return of(this.googleAuthUrl);
    return this.http.get('/api/auth/google/url',{responseType: 'text'}).pipe(map((data: string) => {
       this.googleAuthUrl = data;
       return data;
     }));
  }

  public getAuthenticationInfo(accessToken: string): Observable<TokenResponse> {
    return this.http.post('/api/auth/info',{accessToken}).pipe(map((data: TokenResponse) => {
      if(data){
        this.saveToken(data);
      }
      return data;
     }));
  }

  public getProfile(userId: string){
    return this.http.get(`/api/profile?userId=${userId}`).pipe(map(data => { return <Profile>data}));
  }

}
