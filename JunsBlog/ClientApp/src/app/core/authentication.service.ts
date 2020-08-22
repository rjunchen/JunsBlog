import { Injectable, Output, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { TokenResponse } from '../models/TokenResponse';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {
  @Output() userInfoUpdated: EventEmitter<any> = new EventEmitter();
  private token: string;
  private currentUser: User;
  
  constructor(private http: HttpClient, private router: Router) { }

  public saveToken(tokenResponse: TokenResponse){
    if(tokenResponse){
      this.token = tokenResponse.accessToken;
      this.currentUser = tokenResponse.user;
      localStorage.setItem('accessToken', this.token);
      localStorage.setItem('user', JSON.stringify(tokenResponse.user));
      this.userInfoUpdated.emit(tokenResponse.user);
    }
  }

  public register(formData): Observable<any> {
    return this.http.post('/api/register', formData).pipe(map((data: TokenResponse)=>{
        if(data){
          this.saveToken(data);
        }
    }))
  }

  public login(formData): Observable<any> {
    return this.http.post('/api/login', formData).pipe(map((data: TokenResponse)=>{
      if(data){
        this.saveToken(data);
      }
    }))
  }
}
