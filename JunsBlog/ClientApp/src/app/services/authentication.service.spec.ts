import { TestBed, async } from '@angular/core/testing';

import { AuthenticationService } from './authentication.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { RouterTestingModule } from '@angular/router/testing'
import { HttpResponse } from '@angular/common/http';
import { AuthResponse } from '../models/authentication/authResponse';
import { User } from '../models/authentication/user';
import { Router } from '@angular/router';
import { Profile } from '../models/authentication/profile';

describe('AuthenticationService', () => {
  let service: AuthenticationService;
  let httpMock: HttpTestingController;
  let token: Map<string, string>;
  let routerSpy: jasmine.SpyObj<Router>;
  

  beforeEach(() => {
    const spy = jasmine.createSpyObj('AuthenticationService', ['saveToken'])
    token = new Map<string, string>();

    const naviSpy = jasmine.createSpyObj('Router', ['navigateByUrl']);
    

    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule, RouterTestingModule],
      providers: [ { provide: Router, useValue: naviSpy } ]
    });
    spyOn(localStorage, 'setItem').and.callFake(function(key, value){
      token.set(key, value);
    });

    spyOn(localStorage, 'getItem').and.callFake(function(key){
      return token.get(key);
    });

    httpMock = TestBed.inject(HttpTestingController);
    service = TestBed.inject(AuthenticationService);
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  afterEach(() => {
    httpMock.verify();
  });  


  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('register() should return true', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let testUrl = '/api/register';
      let testData: AuthResponse = new AuthResponse();
      let user:User = new User();
      user.name = 'tester';
      user.email = 'test@gmail.com';
      user.id = '1234';

      testData.accessToken = '1234567890';
      testData.refreshToken = 'asdfghj';
      testData.user = user;

      service.register(user.email, 'myPassword', user.name).subscribe(x=>{
        expect(x).toBeTrue();
        expect(service.getCurrentUser()).toEqual(user);
        expect(service.getToken()).toEqual(testData.accessToken);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(testData);
  }));

  it('login() should return true', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let testUrl = '/api/authenticate';
      let testData: AuthResponse = new AuthResponse();
      let user:User = new User();
      user.name = 'tester';
      user.email = 'test@gmail.com';
      user.id = '1234';

      testData.accessToken = '1234567890';
      testData.refreshToken = 'asdfghj';
      testData.user = user;

      service.login({email: user.email, password: 'myPassword'}).subscribe(x=>{
        expect(x).toBeTrue();
        expect(service.getCurrentUser()).toEqual(user);
        expect(service.getToken()).toEqual(testData.accessToken);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(testData);
  }));

  it('getAuthenticationInfo() should return true', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let testUrl = '/api/auth/info';
      let testData: AuthResponse = new AuthResponse();
      let user:User = new User();
      user.name = 'tester';
      user.email = 'test@gmail.com';
      user.id = '1234';

      testData.accessToken = '1234567890';
      testData.refreshToken = 'asdfghj';
      testData.user = user;

      service.getAuthenticationInfo(testData.accessToken).subscribe(x=>{
        expect(x).toBeInstanceOf(AuthResponse);
        expect(service.getCurrentUser()).toEqual(user);
        expect(service.getToken()).toEqual(testData.accessToken);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(testData);
  }));


  it('logout() should clear the data', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let testUrl = '/api/authenticate';
      let testData: AuthResponse = new AuthResponse();
      let user:User = new User();
      user.name = 'tester';
      user.email = 'test@gmail.com';
      user.id = '1234';

      testData.accessToken = '1234567890';
      testData.refreshToken = 'asdfghj';
      testData.user = user;

      service.login({email: user.email, password: 'myPassword'}).subscribe(x=>{
        expect(x).toBeTrue();
        expect(service.getCurrentUser()).toEqual(user);
        expect(service.getToken()).toEqual(testData.accessToken);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(testData);

      service.logout();
      expect(service.getToken()).toBeNull();
      expect(service.getCurrentUser()).toBeNull();

  }));

  
  it('isLoggedIn() should be true', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let testUrl = '/api/authenticate';
      let testData: AuthResponse = new AuthResponse();
      let user:User = new User();
      user.name = 'tester';
      user.email = 'test@gmail.com';
      user.id = '1234';

      testData.accessToken = '1234567890';
      testData.refreshToken = 'asdfghj';
      testData.user = user;

      service.login({email: user.email, password: 'myPassword'}).subscribe(x=>{
        expect(x).toBeTrue();
        expect(service.getCurrentUser()).toEqual(user);
        expect(service.getToken()).toEqual(testData.accessToken);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(testData);

      expect(service.isLoggedIn()).toBeTrue();
  }));

  it('isLoggedIn() should be false', async(() => {    
      expect(service.isLoggedIn()).toBeFalse();
  }));

  it('getGoogleAuthUrl() should return data', async(() => {    
      let testUrl = '/api/auth/google/url';
      let testData = 'googleUrl';

      service.getGoogleAuthUrl().subscribe(x=>{
        expect(x).toEqual(testData);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('GET');
      req.flush(testData);
  }));

  it('saveToken() should store data', async(() => {    
    let testData: AuthResponse = new AuthResponse();
    let user:User = new User();
    user.name = 'tester';
    user.email = 'test@gmail.com';
    user.id = '1234';

    testData.accessToken = '1234567890';
    testData.refreshToken = 'asdfghj';
    testData.user = user;
    service.saveToken(testData);
    
    expect(service.getToken()).toEqual(testData.accessToken);
    expect(service.getCurrentUser()).toEqual(testData.user);
  }));

  it('getProfile() should return true', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let user:User = new User();
      user.name = 'tester';
      user.email = 'test@gmail.com';
      user.id = '1234';
      let testUrl = `/api/profile?userId=${user.id}`;
      let testData: Profile = new Profile();
      testData.user = user;
     

      service.getProfile(user.id).subscribe(x=>{
        expect(x).toEqual(testData);
        expect(x).toBeInstanceOf(Profile);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('GET');
      req.flush(testData);
  }));

  it('updateProfile() should return true', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);

      let testUrl = `/api/profile/update`;
      let testData: User = new User();
      testData.name = 'tester';
      testData.email = 'test@gmail.com';
      testData.id = '1234';
      testData.image = 'myImage'
     
      service.updateProfile(testData.id, testData.name, testData.email, testData.image).subscribe(x=>{
        expect(x).toEqual(testData);
        expect(x).toBeInstanceOf(User);
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(testData);
  }));

  
  it('sendResetToken() should return not return any data', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);
      let testData: User = new User();
      testData.name = 'tester';
      testData.email = 'test@gmail.com';
      testData.id = '1234';
      testData.image = 'myImage'
      let testUrl = `/api/reset/verifyEmail?email=${testData.email}`;
      
      service.sendResetToken(testData.email).subscribe(x=>{
        expect(x).toBeNull();
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('GET');
      req.flush(null);
  }));

  it('verifyToken() should not return any data', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);
      let email = 'test@gmail.com';
      let token = '1234567';
      let testUrl = `/api/reset/verifyToken?email=${email}&token=${token}`;
      
      service.verifyToken(email, token).subscribe(x=>{
        expect(x).toBeNull();
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('GET');
      req.flush(null);
  }));

  
  it('sendResetToken() should not return any data', async(() => {    
    routerSpy.navigateByUrl.and.returnValue(null);
      let email = 'test@gmail.com';
      let token = '1234567';
      let password = 'myPassword';
      let testUrl = `/api/reset/password`;
      
      service.resetPassword(email, password, token).subscribe(x=>{
        expect(x).toBeNull();
      })

      const req = httpMock.expectOne(testUrl);
      expect(req.request.method).toEqual('POST');
      req.flush(null);
  }));

});
