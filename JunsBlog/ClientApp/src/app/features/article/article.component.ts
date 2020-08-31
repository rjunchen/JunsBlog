import { Component, OnInit } from '@angular/core';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { mergeMap } from 'rxjs/operators';
import { CommentService } from 'src/app/services/comment.service';
import { RankEnum } from 'src/app/models/Enums/rankEnum';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { ArticleRankingDetails } from 'src/app/models/article/articleRankingDetails';
import { ArticleRankingRequest } from 'src/app/models/article/articleRankingRequest';
import { CommentRequest } from 'src/app/models/comment/commentRequest';


@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;
  loading: boolean;
  ranking: ArticleRankingDetails;
  defaultAvatarUrl = './assets/avatar.png';

  constructor(private route: ActivatedRoute,  private articleService: ArticleService,
       private toastr: ToastrService, private commentService: CommentService) {}


  ngOnInit(): void {
    this.loading = true;
    this.route.params.pipe(mergeMap(params => this.articleService.getArticleDetails(params['id']))
    ).subscribe(
      data => { 
        this.article = data;
        this.loading = false;
      },
      err => {
        this.loading = false;
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      }
    )
    this.route.params.pipe(mergeMap(params => this.articleService.GetArticleRanking(params['id']))
    ).subscribe(
      data => { 
        this.ranking = data;
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
    var request = new ArticleRankingRequest(this.article.id, rank);
    this.articleService.rankArticle(request).subscribe(data=> {
      this.ranking = data;
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
    var request = new CommentRequest(this.article);
    this.commentService.showCommentControl(request);
  }
}
