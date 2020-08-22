import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/core/authentication.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  constructor(private auth: AuthenticationService) { }

  ngOnInit(): void {
  }
  
  logout(){
    this.auth.logout()
  }
}
