import { Component, OnInit, OnDestroy, Input } from '@angular/core';
import { CommentDetails } from 'src/app/models/comment/commentDetails';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { Subscription } from 'rxjs';
import { CommentService } from 'src/app/services/comment.service';
import { AlertService } from 'src/app/services/alert.service';
import { RankEnum } from 'src/app/models/enums/rankEnum';
import { CommentRequest } from 'src/app/models/comment/commentRequest';
import { CommentSearchPagingOption } from 'src/app/models/comment/commentSearchPagingOption';
import { CommentSearchPagingResult } from 'src/app/models/comment/CommentSearchPagingResult';
import { CommentRankingRequest } from 'src/app/models/comment/commentRankingRequest';
import { commentSearchOnEnum } from 'src/app/models/enums/commentSearchOnEnum';

@Component({
  selector: 'app-comment-displayer',
  templateUrl: './comment-displayer.component.html',
  styleUrls: ['./comment-displayer.component.scss']
})
export class CommentDisplayerComponent implements OnInit, OnDestroy {
  @Input() comment: CommentDetails;
  @Input() article: ArticleDetails;
  viewRepliesOpen: boolean = false;
  defaultAvatarUrl = './assets/avatar.png';
  isProcessing: boolean;
  loading: boolean;
  commentPagingResult: CommentSearchPagingResult;
  comments: CommentDetails[];
  commentSubscription: Subscription;
  constructor(private commentService: CommentService, private alertService: AlertService) { }


  ngOnDestroy(): void {
    this.commentSubscription.unsubscribe();
  }

  ngOnInit(): void {
    this.comments = [];

    if(this.commentSubscription) return;

    this.commentSubscription = this.commentService.onCommentPosted.subscribe( (commentDetails: CommentDetails)=>{
        if(this.comment.id == commentDetails.parentId){
          this.comment.childrenCommentsCount += 1;
          this.comments.unshift(commentDetails);
        }
    });
  }

  likeComment(){
    this.rank(RankEnum.Like);
  }

  dislikeComment(){
    this.rank(RankEnum.Dislike);
  }

  rank(rank: RankEnum){
    this.isProcessing = true;
    var request = new CommentRankingRequest(this.comment.id, rank);
    this.commentService.rankComment(request).subscribe(data=> {
      this.comment.ranking = data;
      this.isProcessing = false;
    }, err=>{
      this.isProcessing = false;
      this.alertService.alertHttpError(err);
    })
  }


  showCommentControl(comment: CommentDetails){
    var commenterRequest = new CommentRequest(this.article, comment);
    this.commentService.showCommentControl(commenterRequest);
  }


  getMoreReplies(){
      if (!this.loading && this.commentPagingResult.hasNextPage) {
        this.loading = true;
        this.commentPagingResult.searchOption.currentPage += 1;
        this.commentService.searchComments(this.commentPagingResult.searchOption).subscribe(
          data => {
            data.documents.forEach(doc => {
              this.comments.push(doc);
            });
            this.commentPagingResult = data;           
            this.loading = false;
        },
        err => {
          this.loading = false;
          this.alertService.alertHttpError(err);
        }
      )
    }
  }

  viewReplies(){
    this.viewRepliesOpen = !this.viewRepliesOpen;

    if(this.comments.length > 0) return; // Only get the comment calls on the first time when view reply is clicked.
        const intiPage: number = 1;
        const pageSize: number = 10;
        var option = new CommentSearchPagingOption(intiPage, pageSize, this.comment.id, commentSearchOnEnum.ParentId);
        this.loading = true;
        this.commentService.searchComments(option).subscribe(
          data => {
            this.commentPagingResult = data;
            this.commentPagingResult.documents.forEach(doc => {
              this.comments.push(doc);
            });       
            this.loading = false;
        },
        err => {
          this.loading = false;
          this.alertService.alertHttpError(err);
        }
      )
  }
}
