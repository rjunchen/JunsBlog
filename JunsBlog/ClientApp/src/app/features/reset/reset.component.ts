import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { Router } from '@angular/router';
import { MatStepper } from '@angular/material/stepper';
import { ToastrService } from 'ngx-toastr';
import { PasswordResetRequest } from 'src/app/models/passwordResetRequest';

@Component({
  selector: 'app-reset',
  templateUrl: './reset.component.html',
  styleUrls: ['./reset.component.scss']
})
export class ResetComponent implements OnInit, AfterViewInit {

  isLinear = true;
  emailFormGroup: FormGroup;
  tokenFormGroup: FormGroup;
  passwordFormGroup: FormGroup;
  isEditable = false;
  
  hidePassword = true;
  hideConfirmPassword = true;
  isProcessing = false;
  isCompleted = false;

  stepHtmlElements: any;

  constructor(private fb: FormBuilder, private auth: AuthenticationService, private toastr: ToastrService, private router: Router) { }

  ngOnInit(): void {
    this.emailFormGroup = this.fb.group({
      email: ['', Validators.compose([Validators.required, Validators.email]) ]
    });
    this.tokenFormGroup = this.fb.group({
      token: ['', Validators.required]
    });
    this.passwordFormGroup = this.fb.group({
      password: ['', Validators.compose([Validators.required, Validators.minLength(6)])],
      confirmPassword: ['', Validators.compose([Validators.required, Validators.minLength(6)])] },
       { validator: this.passwordMatchValidator });
  }

  ngAfterViewInit(){
    this.stepHtmlElements = (document.getElementsByClassName('mat-step-header'));
    this.setHeaderActive(1);
  }


  setHeaderActive(step: number){
    try{
      for (let index = 0; index < this.stepHtmlElements.length; index++) {
        let backgroundColor = 'white';
        let fontColor = 'grey';
        if(index + 1 === step){
          backgroundColor = '#f44336';
          fontColor = 'white';
        }
        const element = this.stepHtmlElements[index] as HTMLElement;
        element.style.backgroundColor = backgroundColor;
        (element.getElementsByClassName('mat-step-label')[0] as HTMLElement).style.color = fontColor;
      }
    }catch(err){
      console.log(err);
    }
  }

  passwordMatchValidator(control:AbstractControl) {
    const passwordControl: AbstractControl = control.get('password'); 
    const confirmPasswordControl: AbstractControl = control.get('confirmPassword'); 

    if(passwordControl.value !== confirmPasswordControl.value){
       confirmPasswordControl.setErrors({NoPasswordMatch: true});
    }
  }

  onPasswordChanged(){
    this.passwordFormGroup.get('confirmPassword').updateValueAndValidity();
  }


  sendEmail(stepper: MatStepper){
    this.isProcessing = true;
    this.auth.sendResetToken(this.emailFormGroup.value.email).subscribe(x=>{
      this.isProcessing = false;
      stepper.next();
      this.setHeaderActive(2);
    }, err=>{
      this.isProcessing = false;
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }
  verifyToken(stepper: MatStepper){
    this.isProcessing = true;
    this.auth.verifyToken(this.emailFormGroup.value.email, this.tokenFormGroup.value.token).subscribe(x=>{
      this.isProcessing = false;
      stepper.next();
      this.setHeaderActive(3);
    }, err=>{
      this.isProcessing = true;
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }

  resetPassword(stepper: MatStepper){
    this.isProcessing = true;
    var request = new PasswordResetRequest(this.passwordFormGroup.value.password, this.tokenFormGroup.value.token, this.emailFormGroup.value.email)
    this.auth.resetPassword(request).subscribe(x=>{
      this.isProcessing = false;
      stepper.next();
      this.setHeaderActive(4);
    }, err=>{
      this.isProcessing = true;
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }

  login(){
    this.router.navigateByUrl('/login');
  }

}
