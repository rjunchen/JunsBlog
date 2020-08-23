import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { AuthenticationService } from 'src/app/core/authentication.service';

@Component({
  selector: 'app-social',
  templateUrl: './social.component.html',
  styleUrls: ['./social.component.scss']
})
export class SocialComponent implements OnInit {

  constructor(private activatedRoute: ActivatedRoute, private auth: AuthenticationService, 
    private router: Router) {
      this.activatedRoute.queryParams.subscribe(params=>{
        const accessToken = params['accessToken'];
        if(accessToken){
          this.auth.getAuthenticationInfo(accessToken).subscribe(x=> {
            this.router.navigateByUrl('/');
          }, err=>{
            console.log(err);
          })
        }
      })
    }

  ngOnInit(): void {
  }

}
