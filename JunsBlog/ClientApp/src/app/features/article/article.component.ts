import { Component, OnInit } from '@angular/core';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute } from '@angular/router';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { mergeMap } from 'rxjs/operators';

import { RankingRequest } from 'src/app/models/RankingRequest';
import { RankEnum } from 'src/app/models/rankEnum';
import { CommentService } from 'src/app/services/comment.service';
import { CommenterRequest } from 'src/app/models/commenterRequest';
import { ArticleRankingDetails } from 'src/app/models/articleRankingDetails';
import { CommentTypeEnum } from 'src/app/models/CommentTypeEnum';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;
  ranking: ArticleRankingDetails;
  defaultAvatarUrl = './assets/avatar.png';

  constructor(private route: ActivatedRoute,  private articleService: ArticleService,
       private toastr: ToastrService, private commentService: CommentService) {}


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
    var request = new RankingRequest(this.article.id, rank);
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
    var request = new CommenterRequest(this.article.id, this.article.comments, CommentTypeEnum.Article);
    this.commentService.showCommentControl(request);
  }
}
