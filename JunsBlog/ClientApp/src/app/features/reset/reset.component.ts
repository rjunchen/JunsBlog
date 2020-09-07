import { Component, OnInit, AfterViewInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { Router } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { MatStepper } from '@angular/material/stepper';
import { CustomValidationService } from 'src/app/services/custom-validation.service';

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

  constructor(private fb: FormBuilder, private auth: AuthenticationService, private customValidator: CustomValidationService, private alertService: AlertService, private router: Router) { }

  ngOnInit(): void {
    this.emailFormGroup = this.fb.group({
      email: ['', Validators.compose([Validators.required, this.customValidator.emailPatternValidator()]) ]
    });
    this.tokenFormGroup = this.fb.group({
      token: ['', Validators.required]
    });
    this.passwordFormGroup = this.fb.group({
      password: ['', Validators.compose([Validators.required, Validators.minLength(8),
        this.customValidator.hasNumber(), this.customValidator.hasUppercaseLetter()])],
      confirmPassword: ['', Validators.required] },
      { validator: this.customValidator.MatchPassword('password', 'confirmPassword') });
  }

  ngAfterViewInit(){
    this.stepHtmlElements = (document.getElementsByClassName('mat-step-header'));
    this.setHeaderActive(1);
  }

  get emailFormControl() {
    return this.emailFormGroup.controls;
  }

  get passwordFormControl() {
    return this.passwordFormGroup.controls;
  }

  isInvalid(control: AbstractControl){
      if(!control) return false;
      if(control.invalid && control.touched) return true;
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

  sendEmail(stepper: MatStepper){
    this.isProcessing = true;
    this.auth.sendResetToken(this.emailFormGroup.value.email).subscribe(x=>{
      this.isProcessing = false;
      stepper.next();
      this.setHeaderActive(2);
    }, err=>{
      this.isProcessing = false;
      this.alertService.alertHttpError(err);
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
      this.alertService.alertHttpError(err);
    })
  }

  resetPassword(stepper: MatStepper){
    this.isProcessing = true;
    this.auth.resetPassword( this.emailFormGroup.value.email, this.passwordFormGroup.value.password, this.tokenFormGroup.value.token).subscribe(x=>{
      this.isProcessing = false;
      stepper.next();
      this.setHeaderActive(4);
    }, err=>{
      this.isProcessing = false;
      this.alertService.alertHttpError(err);
    })
  }

  login(){
    this.router.navigateByUrl('/login');
  }

}
