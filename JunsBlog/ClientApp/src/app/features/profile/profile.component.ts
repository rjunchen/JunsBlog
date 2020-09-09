import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { ActivatedRoute } from '@angular/router';
import { User } from 'src/app/models/authentication/user';
import { Profile } from 'src/app/models/authentication/profile';
import { AlertService } from 'src/app/services/alert.service';
import { ArticleService } from 'src/app/services/article.service';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';
import { ArticleFilterEnum } from 'src/app/models/enums/articleFilterEnum';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  profile: Profile;
  showEditor: boolean;
  currentUser: User;
  selectedTab: string;
  searchOption: ArticleSearchPagingOption;

  constructor(private auth: AuthenticationService, private articleService: ArticleService, private route: ActivatedRoute, private alertService: AlertService) { }

  ngOnInit(): void {
    this.searchOption = new ArticleSearchPagingOption();
    this.currentUser = this.auth.getCurrentUser();
    this.route.paramMap.subscribe(x=>{
      const profilerId = x.get("id")
      this.searchOption.profilerId = profilerId;
      this.auth.getProfile(profilerId).subscribe(data=>{
        this.profile = data;
      }, err=>{
        this.logout();
        this.alertService.alertHttpError(err);
      })
     });
  }

  logout(){
    this.auth.logout()
  }

  toggleProfileEditor(){
    this.showEditor = !this.showEditor;
  }

  isAuthorized(){
    if(this.currentUser && this.currentUser.id == this.profile.user.id)
      return true;
    else
      return false;
  }

  showMyLikes(){
    this.selectedTab = 'myLikes';
    this.searchOption.filter = ArticleFilterEnum.MyLikes;
    this.articleService.SearchClicked(this.searchOption);
  }

  showMyArticles(){
    this.selectedTab = 'myArticles';
    this.searchOption.filter = ArticleFilterEnum.MyArticles;
    this.articleService.SearchClicked(this.searchOption);
  }

  showMyFavorites(){
    this.selectedTab = 'myFavorites';
    this.searchOption.filter = ArticleFilterEnum.MyFavorites;
    this.articleService.SearchClicked(this.searchOption);
  }

  showSettings(){
    this.selectedTab = 'settings';
    this.alertService.info('Setting feature is under development')
  }

}
