import { Component, OnInit, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommentService } from 'src/app/services/comment.service';
import { User } from 'src/app/models/user';
import { commentSearchOnEnum } from 'src/app/models/Enums/commentSearchOnEnum';
import { CommentDetails } from 'src/app/models/comment/commentDetails';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { CommentSearchPagingResult } from 'src/app/models/comment/commentSearchPagingResult';
import { CommentSearchPagingOption } from 'src/app/models/comment/commentSearchPaingOption';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() article: ArticleDetails;
  currentUser: User;

 commentPagingResult: CommentSearchPagingResult;
 defaultAvatarUrl = './assets/avatar.png';

 throttle = 300;
 scrollDistance = 1;
 scrollUpDistance = 2;
 loading = false;
 displayComments: CommentDetails[] = [];

 constructor(private commentService: CommentService, private toastr: ToastrService) { }

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
     if(err.status === 400){
       this.toastr.warning(err.error.message, err.statusText);
     }
     else{
       this.toastr.error('Unknown error occurred, please try again later');
     }
   });

   this.commentService.onCommentPosted.subscribe( (commentDetails : CommentDetails) => {
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
         if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
       }
     )
   }
 }
}
