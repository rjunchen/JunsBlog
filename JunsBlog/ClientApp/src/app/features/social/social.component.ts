import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/services/authentication.service';

@Component({
  selector: 'app-social',
  templateUrl: './social.component.html',
  styleUrls: ['./social.component.scss']
})
export class SocialComponent implements OnInit {

  inProcess: boolean;
  constructor(private activatedRoute: ActivatedRoute, private auth: AuthenticationService, 
    private router: Router) {
      this.activatedRoute.queryParams.subscribe(params=>{
        const accessToken = params['accessToken'];
        if(accessToken){
          this.inProcess = true;
          this.auth.getAuthenticationInfo(accessToken).subscribe(x=> {
            this.inProcess = false;
            this.router.navigateByUrl('/');
          }, err=>{
            this.inProcess = false;
            console.log(err);
            this.router.navigateByUrl('/');
          })
        }
      })
    }

  ngOnInit(): void {
  }

}
