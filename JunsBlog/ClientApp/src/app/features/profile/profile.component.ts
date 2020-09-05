import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/authentication/user';
import { Profile } from 'src/app/models/authentication/profile';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  profile: Profile;
  showEditor: boolean;
  currentUser: User;
  
  constructor(private auth: AuthenticationService, private route: ActivatedRoute) { }

  ngOnInit(): void {

    this.currentUser = this.auth.getCurrentUser();

    this.route.paramMap.subscribe(x=>{
      const profilerId = x.get("id")
      this.auth.getProfile(profilerId).subscribe(data=>{
        console.log(data);
        this.profile = data;
      }, err=>{
        this.logout();
      })
     });
  }

  logout(){
    this.auth.logout()
  }

  toggleProfileEditor(){
    this.showEditor = !this.showEditor;
  }
}
