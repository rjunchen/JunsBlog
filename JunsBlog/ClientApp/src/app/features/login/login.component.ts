import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router } from '@angular/router';
import { CustomValidationService } from 'src/app/services/custom-validation.service';
import { AlertService } from 'src/app/services/alert.service';

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
    private fb: FormBuilder, private customValidator: CustomValidationService,
    private alertService: AlertService) { this.createForm() }

  createForm() {
    this.loginForm = this.fb.group({
      email: ['', Validators.compose([Validators.required, this.customValidator.emailPatternValidator()])],
      password: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.auth.getGoogleAuthUrl().subscribe( x=>{
      this.googleAuthUrl = x;
    }, err =>{
      
    })
  }

  get loginFormControl() {
    return this.loginForm.controls;
  }

  isInvalid(control: AbstractControl){
    if(!control) return false;
    if(control.invalid && control.touched) return true;
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
        this.alertService.alertHttpError(err);
      }
    );
  }
}
