import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service'
import { AuthenticationService } from 'src/app/services/authentication.service';
import { User } from 'src/app/models/authentication/user';
import { Location } from '@angular/common'
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';
import { Router } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  currentUser: User;
  searchKey: string;
  showMobileSearch: boolean;

  constructor(public auth: AuthenticationService, private router: Router,
    private eventService: EventService, private location: Location) { 
    this.auth.onUserInfoUpdated.subscribe(data => this.currentUser = data);
  }

  ngOnInit(): void {
    this.searchKey = '';
    this.currentUser = this.auth.getCurrentUser();
  }

  toggleSidebar(){
    this.eventService.toggleSidebar();
  }

  goBack(){
    this.location.back();
  }

  get isValidSearchKey(){
    return this.searchKey.trim().length > 0;
  }

  search(){
    if(this.isValidSearchKey)
      this.router.navigateByUrl(`/search/${this.searchKey.trim()}`);
    this.showMobileSearch = false;
  }

  toggleMobileSearch(){
    this.showMobileSearch = !this.showMobileSearch;
  }
}
