import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';
import { Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  private googleAuthUrl: string;
  
  constructor(private http: HttpClient) { }

  public register(reqRequest: any){
    return this.http.post('/api/register', reqRequest).pipe(map((data: any)=>{
      if(data){
        //this.saveToken(data);
      }
      return data;
    }))
  }

  public login(formData): Observable<any> {
    return this.http.post('/api/authenticate', formData).pipe(map((data: any)=>{
      if(data){
        //this.saveToken(data);
      }
      return data;
    }))
  }

  public getGoogleAuthUrl(): Observable<string> {
    if(this.googleAuthUrl)
      return of(this.googleAuthUrl);
    return this.http.get('/api/auth/google/url',{responseType: 'text'}).pipe(map((data: string) => {
       this.googleAuthUrl = data;
       return data;
     }));
  }
}
