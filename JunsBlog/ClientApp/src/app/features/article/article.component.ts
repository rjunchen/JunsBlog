import { Component, OnInit } from '@angular/core';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { mergeMap } from 'rxjs/operators';
import { RankingResponse } from 'src/app/models/rankingResponse';
import { RankingRequest } from 'src/app/models/RankingRequest';
import { RankEnum } from 'src/app/models/rankEnum';
import { User } from 'src/app/models/user';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { CommentService } from 'src/app/services/comment.service';
import { CommenterRequest } from 'src/app/models/commenterRequest';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;
  ranking: RankingResponse;
  defaultAvatarUrl = './assets/avatar.png';

  constructor(private auth: AuthenticationService, private route: ActivatedRoute,  private articleService: ArticleService,
     private router: Router, private toastr: ToastrService, private commentService: CommentService) {}


  ngOnInit(): void {



    this.route.params.pipe(mergeMap(params => this.articleService.getArticle(params['id']))
    ).subscribe(
      data => { 
        this.article = data;
      },
      err => {
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      }
    )
  }

  like(){
    this.isProcessing = true;
    var request = new RankingRequest();
    request.articleId = this.article.id;
    request.rank = RankEnum.Like;
    this.articleService.rankArticle(request).subscribe(data=> {
      this.article.ranking = data;
      this.isProcessing = false;
    }, err=>{
      this.isProcessing = false;
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }

  favor(){
    this.isProcessing = true;
    var request = new RankingRequest();
    request.articleId = this.article.id;
    request.rank = RankEnum.Favor;
    this.articleService.rankArticle(request).subscribe(data=> {
      this.article.ranking = data;
      this.isProcessing = false;
    }, err=>{
      this.isProcessing = false;
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }

  dislike(){
    this.isProcessing = true;
    var request = new RankingRequest();
    request.articleId = this.article.id;
    request.rank = RankEnum.Dislike;
    this.articleService.rankArticle(request).subscribe(data=> {
      this.article.ranking = data;

      this.isProcessing = false;
    }, err=>{
      this.isProcessing = false;
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }

  showCommenter(){
    var request = new CommenterRequest();
    request.callerId = this.article.id;
    request.comments = this.article.comments;
    request.isCallerArticle = true;
    this.commentService.showCommentControl(request);
  }
}
