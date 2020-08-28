import { Component, OnInit, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommentService } from 'src/app/services/comment.service';
import { User } from 'src/app/models/user';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { CommentSearchPagingResult } from 'src/app/models/commentSearchPagingResult';
import { commentSearchOnEnum } from 'src/app/models/Enums/commentSearchOnEnum';
import { CommentDetails } from 'src/app/models/commentDetails';

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
   this.commentService.searchComments(intiPage, pageSize, this.article.id, commentSearchOnEnum.ParentId).subscribe(x=>{
     this.displayComments = x.documents;
     this.commentPagingResult = x;
     console.log(x);
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
   console.log('hehhe');
   if(this.commentPagingResult && this.commentPagingResult.hasNextPage && !this.loading){
     this.loading = true;
     this.commentService.searchComments(this.commentPagingResult.currentPage + 1 , this.commentPagingResult.pageSize, 
      this.commentPagingResult.searchKey, this.commentPagingResult.searchOn, this.commentPagingResult.sortBy, this.commentPagingResult.sortOrder).subscribe(
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
