import { Injectable } from '@angular/core';
import { AuthenticationService } from './authentication.service';
import { HttpRequest, HttpHandler, HttpEvent, HttpErrorResponse } from '@angular/common/http';
import { Observable, throwError, of } from 'rxjs';
import { Router } from '@angular/router';
import { catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class InterceptorService {

  constructor(public auth: AuthenticationService, private router: Router) { }

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    if(this.auth.getCurrentUser()){
      req = req.clone({
        setHeaders: { Authorization: `Bearer ${this.auth.getToken()}` }
      }); 
    } 
    return next.handle(req).pipe( catchError(x => this.handleAuthError(x)));
  }

  private handleAuthError(err: HttpErrorResponse): Observable<any>{
    if( err.status === 401 || err.status === 403){
      this.router.navigateByUrl('/login');
      return of(err.message);
    }
    return throwError(err);
  }
}
