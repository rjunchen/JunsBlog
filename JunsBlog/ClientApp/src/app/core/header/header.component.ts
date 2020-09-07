import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service'
import { AuthenticationService } from 'src/app/services/authentication.service';
import { User } from 'src/app/models/authentication/user';
import { Location } from '@angular/common'

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  currentUser: User;

  constructor(public auth: AuthenticationService, private eventService: EventService, private location: Location) { 
    this.auth.onUserInfoUpdated.subscribe(data => this.currentUser = data);
  }

  ngOnInit(): void {
    this.currentUser = this.auth.getCurrentUser();
  }

  toggleSidebar(){
    this.eventService.toggleSidebar();
  }

  goBack(){
    this.location.back();
  }
}
