import { Component, OnInit } from '@angular/core';
import { Router } from "@angular/router";
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/authentication.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  hidePassword = true;
  loginForm: FormGroup;

  constructor(private auth: AuthenticationService, private router: Router, 
    private fb: FormBuilder) { this.createForm() }

  createForm() {
    this.loginForm = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.required]
    })
  }

  ngOnInit(): void {

  }

  onSubmit(){
    this.auth.login(this.loginForm.value).subscribe(
      () => {
         this.router.navigateByUrl('/');
      },
      (err) => {
        if (err.status === 401) {     
         // this.toastr.warning(err.error, 'Unauthorized', {positionClass:'toast-top-full-width', timeOut:10000});
        } else {
         // this.toastr.error('Unknown error occurred, please try again later', '', {positionClass:'toast-top-full-width', timeOut:10000});
        }
      }
    );
  }

}
