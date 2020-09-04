import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient) { }

  public register(reqRequest: any){
    return this.http.post('/api/register', reqRequest).pipe(map((data: any)=>{
      if(data){
        //this.saveToken(data);
      }
      return data;
    }))
  }
}
