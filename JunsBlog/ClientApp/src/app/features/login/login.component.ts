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
  inProcess: boolean;

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
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }

  onSubmit(){
    this.inProcess = true;
    this.auth.login(this.loginForm.value).subscribe(
      () => {
        this.inProcess = false;
         this.router.navigateByUrl('/');
      },
      (err) => {
        this.inProcess = false;
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      }
    );
  }

}
