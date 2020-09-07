import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { User } from 'src/app/models/authentication/user';
import { CommentService } from 'src/app/services/comment.service';
import { CommentRequest } from 'src/app/models/comment/commentRequest';
import { CommentSearchPagingOption } from 'src/app/models/comment/commentSearchPagingOption';
import { commentSearchOnEnum } from 'src/app/models/enums/commentSearchOnEnum';
import { AlertService } from 'src/app/services/alert.service';
import { CommentSearchPagingResult } from 'src/app/models/comment/CommentSearchPagingResult';
import { CommentDetails } from 'src/app/models/comment/commentDetails';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit, OnDestroy {
  @Input() article: ArticleDetails;
  currentUser: User;

  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  loading = false;
  displayComments: any[] = [];
  commentPagingResult: CommentSearchPagingResult;
  commentSubscription: Subscription;

  constructor(private commentService: CommentService, private alertService: AlertService) { }

  ngOnDestroy(): void {
    this.commentSubscription.unsubscribe();
  }

  ngOnInit(): void {
    const intiPage: number = 1;
    const pageSize: number = 10;
    this.loading = true;
    var option = new CommentSearchPagingOption(intiPage, pageSize, this.article.id, commentSearchOnEnum.ParentId);
    this.commentService.searchComments(option).subscribe(x=>{
      this.displayComments = x.documents;
      this.commentPagingResult = x;
      this.loading = false;
    }, err =>{
      this.loading = false;
      this.alertService.alertHttpError(err);
    });
 
   this.commentSubscription = this.commentService.onCommentPosted.subscribe( (commentDetails : CommentDetails) => {
       this.article.commentsCount += 1; // counts the total comments including all the children comments
       if(commentDetails.articleId == commentDetails.parentId)  // This is a comment on the article
         this.displayComments.unshift(commentDetails); 
    })
  }

  onScrollDown () {
    if(this.commentPagingResult && this.commentPagingResult.hasNextPage && !this.loading){
      this.loading = true;
      this.commentPagingResult.searchOption.currentPage += 1;
      this.commentService.searchComments(this.commentPagingResult.searchOption).subscribe(
        data => {        
            data.documents.forEach(doc => {
             this.displayComments.push(doc);
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
}
