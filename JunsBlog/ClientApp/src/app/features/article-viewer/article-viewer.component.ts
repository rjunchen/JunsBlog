import { Component, OnInit } from '@angular/core';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from 'src/app/services/article.service';
import { mergeMap } from 'rxjs/operators';
import { AlertService } from 'src/app/services/alert.service';
import { User } from 'src/app/models/authentication/user';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { RankEnum } from '../../models/enums/rankEnum'
import { ArticleRankingDetails } from 'src/app/models/article/articleRankingDetails';
import { CommentRequest } from 'src/app/models/comment/commentRequest';
import { CommentService } from 'src/app/services/comment.service';


@Component({
  selector: 'app-article-viewer',
  templateUrl: './article-viewer.component.html',
  styleUrls: ['./article-viewer.component.scss']
})
export class ArticleViewerComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;
  loading: boolean;
  currentUser: User;
  ranking: ArticleRankingDetails;

  constructor(private route: ActivatedRoute,  private articleService: ArticleService, private alertService: AlertService,
    private router: Router, private auth: AuthenticationService, private commentService: CommentService) { }

  ngOnInit(): void {
    this.loading = true;
    this.currentUser = this.auth.getCurrentUser();
    this.route.params.pipe(mergeMap(params => this.articleService.getArticleDetails(params['id']))
    ).subscribe(
      data => { 
        this.article = data;
        this.loading = false;
      },
      err => {
        this.loading = false;
        this.alertService.alertHttpError(err);
      }
    );

    this.route.params.pipe(mergeMap(params => this.articleService.getArticleRankingDetails(params['id']))
    ).subscribe(
      data => { 
        this.ranking = data;
        this.loading = false;
      },
      err => {
        this.loading = false;
        this.alertService.alertHttpError(err);
      }
    );

  }

  edit(){
    this.router.navigateByUrl(`/editor/${this.article.id}`);
  }

  like(){
    this.rank(RankEnum.Like);
  }

  favor(){
    this.rank(RankEnum.Favor);
  }

  dislike(){
    this.rank(RankEnum.Dislike);
  }

  rank(rank: RankEnum){
    this.isProcessing = true;
    this.articleService.rankArticle(this.article.id, rank).subscribe(data=> {
      this.ranking = data;
      this.isProcessing = false;
    }, err=>{
      this.isProcessing = false;
      this.alertService.alertHttpError(err);
    })
  }

  showCommenter(){
    var request = new CommentRequest(this.article);
    this.commentService.showCommentControl(request);
  }

}
