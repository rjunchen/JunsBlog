import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormGroup, Validators, AbstractControl, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthenticationService } from './../../services/authentication.service'
import { CustomValidationService } from './../../services/custom-validation.service'

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class RegisterComponent implements OnInit {

  regForm: FormGroup;
  hidePassword = true;
  hideConfirmPassword = true;
  inProcess: boolean;

  constructor(private auth: AuthenticationService, private router: Router, private fb: FormBuilder,
    private customValidator: CustomValidationService) { 
    this.createForm();
  }

  ngOnInit(): void {

  }

  createForm() {
    this.regForm = this.fb.group({
      name: ['', Validators.required],
      email: ['', Validators.compose([Validators.required, this.customValidator.emailPatternValidator()])],
      password: ['', Validators.compose([Validators.required, Validators.minLength(8),
         this.customValidator.hasNumber(), this.customValidator.hasUppercaseLetter()])],
      confirmPassword: ['', Validators.required],
    }, { validator: this.customValidator.MatchPassword('password', 'confirmPassword') });
  }

  get registerFormControl() {
    return this.regForm.controls;
  }

  isInvalid(control: AbstractControl){
      if(!control) return false;
      if(control.invalid && control.touched) return true;
  }

  onSubmit(){
    this.inProcess = true;
    this.auth.register(this.regForm.value).subscribe( res =>{
      this.inProcess = false;
        this.router.navigateByUrl("/");
    }, err => {
      this.inProcess = false;

    })
  }

}
