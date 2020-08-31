import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { Profile } from 'src/app/models/profile/profile';
import { ActivatedRoute } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';
import { ArticleService } from 'src/app/services/article.service';
import { ArticleFilterEnum } from 'src/app/models/enums/articleFilterEnum';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss']
})
export class ProfileComponent implements OnInit {

  defaultAvatarUrl = './assets/avatar.png';
  profile: Profile;
  isProcessing: boolean;
  searchOption: ArticleSearchPagingOption;
  pageSize: number = 10;

  constructor(private auth: AuthenticationService, private route: ActivatedRoute,
    private articleService: ArticleService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.searchOption = new ArticleSearchPagingOption();
    this.route.paramMap.subscribe(x=>{
      var userId = x.get("id")
      this.auth.getProfile(userId).subscribe(data=>{
        console.log(data);
        this.profile = data;
      }, err=>{
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      })
    });
  }
  
  logout(){
    this.auth.logout()
  }

  showMyLikes(){
    this.searchOption.filter = ArticleFilterEnum.MyLikes;
    this.search();
  }

  showMyArticles(){
    this.searchOption.filter = ArticleFilterEnum.MyArticles;
    this.search();
  }

  showMyFavorites(){
    console.log("clicked favorite");
    this.searchOption.filter = ArticleFilterEnum.MyFavorites;
    this.search();
  }

  search(){
    this.articleService.SearchClicked(this.searchOption);
  }

}
