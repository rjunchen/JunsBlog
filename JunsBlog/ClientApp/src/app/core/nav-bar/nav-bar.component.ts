import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl } from '@angular/forms';
import { User } from 'src/app/models/user';
import { AuthenticationService } from '../authentication.service';

@Component({
  selector: 'app-nav-bar',
  templateUrl: './nav-bar.component.html',
  styleUrls: ['./nav-bar.component.scss']
})
export class NavBarComponent implements OnInit {
  imageUrl = './assets/avatar.png';
  logoUrl = './assets/logo.png';
  currentUser: User;
  showSearchBar: boolean;
  searchForm: FormGroup;

  constructor(public auth: AuthenticationService, private fb: FormBuilder, private router: Router){
    this.auth.userInfoUpdated.subscribe(data => this.currentUser = data);
    this.searchForm = this.fb.group({
       searchKeyWord: ['', Validators.required]
    })
  }

  ngOnInit(): void {
    this.currentUser = this.auth.getCurrentUser();
  }

  search(){
    const searchControl: AbstractControl = this.searchForm.controls['searchKeyWord']; 
  }

  goToPage(page: string) {
    this.router.navigateByUrl(`/${page}`);
  }
}
