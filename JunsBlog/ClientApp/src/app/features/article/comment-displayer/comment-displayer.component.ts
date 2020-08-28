import { Component, OnInit, Input } from '@angular/core';
import { CommentDetails } from 'src/app/models/commentDetails';
import { CommentService } from 'src/app/services/comment.service';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { ToastrService } from 'ngx-toastr';
import { CommentRankingRequest } from 'src/app/models/commentRankingRequest';
import { RankEnum } from 'src/app/models/Enums/rankEnum';
import { commentSearchOnEnum } from 'src/app/models/Enums/commentSearchOnEnum';
import { CommentSearchPagingResult } from 'src/app/models/commentSearchPagingResult';
import { CommentRequest } from 'src/app/models/commentRequest';


@Component({
  selector: 'app-comment-displayer',
  templateUrl: './comment-displayer.component.html',
  styleUrls: ['./comment-displayer.component.scss']
})
export class CommentDisplayerComponent implements OnInit {
  @Input() comment: CommentDetails;
  @Input() article: ArticleDetails;
  viewRepliesOpen: boolean = false;
  defaultAvatarUrl = './assets/avatar.png';
  isProcessing: boolean;
  loading: boolean;
  commentPagingResult: CommentSearchPagingResult;
  comments: CommentDetails[];

  constructor(private commentService: CommentService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.comments = [];
    this.commentService.onCommentPosted.subscribe( (commentDetails: CommentDetails)=>{
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
      if (err.status === 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
  }


  showCommentControl(comment: CommentDetails){
    console.log(comment);
    var commenterRequest = new CommentRequest(this.article, comment);
    this.commentService.showCommentControl(commenterRequest);
  }


  getMoreReplies(){
      if (!this.loading && this.commentPagingResult.hasNextPage) {
        this.loading = true;
        this.commentService.searchComments(this.commentPagingResult.currentPage + 1, this.commentPagingResult.pageSize,
          this.commentPagingResult.searchKey, this.commentPagingResult.searchOn, this.commentPagingResult.sortBy, this.commentPagingResult.sortOrder).subscribe(
          data => {
            data.documents.forEach(doc => {
              this.comments.push(doc);
            });
            this.commentPagingResult = data;           
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
    }
  }

  viewReplies(){
    this.viewRepliesOpen = !this.viewRepliesOpen;

    if(this.comments.length > 0) return; // Only get the comment calls on the first time when view reply is clicked.
        const intiPage: number = 1;
        const pageSize: number = 10;
        this.commentService.searchComments(intiPage, pageSize, this.comment.id, commentSearchOnEnum.ParentId).subscribe(
          data => {
            console.log(data);
            this.commentPagingResult = data;
            this.commentPagingResult.documents.forEach(doc => {
              this.comments.push(doc);
            });       
            this.loading = false;
        },
        err => {
          this.loading = false;
          if(err.status === 400){
            this.toastr.warning(err.error.message, err.statusText);
          }
          else{
            this.toastr.error('Unknown error occurred, please try again later');
          }
        }
      )
  }
}
