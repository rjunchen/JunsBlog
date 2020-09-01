import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { User } from 'src/app/models/user';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { ImageCroppedEvent } from 'ngx-image-cropper';
import { UserInfoUpdateRequest } from 'src/app/models/userInfoUpdateRequest';

@Component({
  selector: 'app-profile-editor',
  templateUrl: './profile-editor.component.html',
  styleUrls: ['./profile-editor.component.scss']
})
export class ProfileEditorComponent implements OnInit {
  @Input() user: User;
  @Output() hideEditor = new EventEmitter<string>();
  defaultAvatarUrl = './assets/avatar.png';

  imageChangedEvent: any = '';
  croppedImage: any = '';
  userForm: FormGroup;
  avatarSrc: any;
  uploading: boolean;

  constructor(private auth: AuthenticationService,  private fb: FormBuilder, private toastr: ToastrService) { 
    this.createForm();
  }

  ngOnInit(): void {
    this.openCloseEditor();
  }

  createForm() {
    this.userForm = this.fb.group({
      displayName: ['', Validators.required],
      email: ['', Validators.compose([Validators.required, Validators.email])]
    })
  }

  openCloseEditor(){
    this.avatarSrc = this.user.image;
    const displayNameControl: AbstractControl = this.userForm.controls['displayName']; 
    const emailControl: AbstractControl = this.userForm.controls['email']; 
    displayNameControl.setValue(this.user.name); 
    emailControl.setValue(this.user.email); 
  }

  onSave(){
    const displayNameControl: AbstractControl = this.userForm.controls['displayName']; 
    const emailControl: AbstractControl = this.userForm.controls['email']; 

 
    var userRequest = new UserInfoUpdateRequest(this.user.id, displayNameControl.value, emailControl.value, this.avatarSrc);

    this.auth.updateUserInfo(userRequest).subscribe(x=>{
      this.user.email = x.email;
      this.user.name = x.name;
      this.user.image = x.image;
      this.hideEditor.emit();
    }, err=>{
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    });
  }

  onClick(){
 
  }
  onCancel(){
    this.hideEditor.emit();
  }

  fileChangeEvent(event: any): void {
      this.imageChangedEvent = event;
      this.uploading = true;
  }
  imageCropped(event: ImageCroppedEvent) {
    this.avatarSrc = event.base64;
  }
  
  cropperReady() {
    console.log('uploaded');
    this.uploading = false;
  }

  loadImageFailed() {
    this.toastr.error('Failed to load image');
  }

}
