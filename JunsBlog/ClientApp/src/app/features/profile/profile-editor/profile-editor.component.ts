import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { User } from 'src/app/models/authentication/user';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { CustomValidationService } from 'src/app/services/custom-validation.service';

@Component({
  selector: 'app-profile-editor',
  templateUrl: './profile-editor.component.html',
  styleUrls: ['./profile-editor.component.scss']
})
export class ProfileEditorComponent implements OnInit {
  @Input() user: User;
  @Output() onEditorExit = new EventEmitter<string>();

  imageChangedEvent: any = '';
  croppedImage: any = '';
  userForm: FormGroup;
  avatarSrc: any;
  uploading: boolean;
  isAvatarDirty: boolean;

  constructor(private auth: AuthenticationService,  private fb: FormBuilder, private customValidator: CustomValidationService) {}

  ngOnInit(): void {
    this.createForm();
    this.avatarSrc = this.user.image;
  }

  createForm() {
    this.userForm = this.fb.group({
      displayName: [this.user.name, Validators.required],
      email: [this.user.email, Validators.compose([Validators.required, this.customValidator.emailPatternValidator()])]
    })
  }

  get userFormControl() {
    return this.userForm.controls;
  }

  isInvalid(control: AbstractControl){
    if(!control) return false;
    if(control.invalid && control.dirty) return true;
  }

  onSave(){
    const displayNameControl: AbstractControl = this.userForm.controls['displayName']; 
    const emailControl: AbstractControl = this.userForm.controls['email']; 
    
    this.auth.updateProfile(this.user.id, displayNameControl.value, emailControl.value, this.avatarSrc).subscribe(x=>{
      this.user.email = x.email;
      this.user.name = x.name;
      this.user.image = x.image;
      this.onEditorExit.emit();
    }, err=>{
      // if (err.status === 400) {     
      //   this.toastr.warning(err.error.message, err.statusText);
      // } else {
      //   this.toastr.error('Unknown error occurred, please try again later');
      // }
    });
    this.onEditorExit.emit();
  }

  canSave(){
    return (this.isAvatarDirty || this.userForm.controls['email'].dirty 
    || this.userForm.controls['displayName'].dirty) && !this.uploading && this.userForm.valid;
  }

  onCancel(){
    this.onEditorExit.emit();
  }

  fileChangeEvent(event: any): void {
    this.imageChangedEvent = event;
    this.uploading = true;
  }
  imageCropped(event: ImageCroppedEvent) {
    this.avatarSrc = event.base64;
    this.isAvatarDirty = true;
  }

  cropperReady() {
    this.uploading = false;
  }

  loadImageFailed() {
    //this.toastr.error('Failed to load image');
  }

}
