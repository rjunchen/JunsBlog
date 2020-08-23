import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/authentication.service';

import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  hidePassword = true;
  loginForm: FormGroup;
  googleAuthUrl: string;

  constructor(private auth: AuthenticationService, private router: Router, 
    private fb: FormBuilder, private toastr: ToastrService) { this.createForm() }

  createForm() {
    this.loginForm = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.auth.getGoogleAuthUrl().subscribe( x=>{
      this.googleAuthUrl = x;
    }, err =>{
      console.log(err);
    })
  }

  onSubmit(){
    this.auth.login(this.loginForm.value).subscribe(
      () => {
         this.router.navigateByUrl('/');
      },
      (err) => {
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText,  {positionClass:'toast-top-full-width', timeOut:10000});
        } else {
          this.toastr.error('Unknown error occurred, please try again later', '', {positionClass:'toast-top-full-width', timeOut:10000});
        }
      }
    );
  }

}
