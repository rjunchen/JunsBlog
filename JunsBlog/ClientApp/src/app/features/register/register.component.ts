import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  regForm: FormGroup;
  hidePassword = true;
  hideConfirmPassword = true;

  constructor(private auth: AuthenticationService, private router: Router, 
    private fb: FormBuilder, private toastr: ToastrService) { this.createForm()}

  ngOnInit(): void {

  }

  createForm() {
    this.regForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', Validators.compose([Validators.required, Validators.email])],
      password: ['', Validators.compose([Validators.required, Validators.minLength(6)])],
      confirmPassword: ['', Validators.compose([Validators.required, Validators.minLength(6)])],
    }, { validator: this.passwordMatchValidator })
  }

  passwordMatchValidator(control:AbstractControl) {
    const passwordControl: AbstractControl = control.get('password'); 
    const confirmPasswordControl: AbstractControl = control.get('confirmPassword'); 

    if(passwordControl.value !== confirmPasswordControl.value){
       confirmPasswordControl.setErrors({NoPasswordMatch: true});
    }
  }

  onPasswordChanged(){
    this.regForm.get('confirmPassword').updateValueAndValidity();
  }

  onSubmit(){
    
    this.auth.register(this.regForm.value).subscribe( res =>{
        this.router.navigateByUrl("/");
    }, err => {
      if(err.status === 409){
        this.toastr.warning('The email have already been registered by other user.', '', {positionClass:'toast-top-full-width', timeOut:8000});
      }
      else{
        this.toastr.error('Unknown error occurred, please try again later', '', {positionClass:'toast-top-full-width', timeOut:10000});
      }
    })
  }

}
