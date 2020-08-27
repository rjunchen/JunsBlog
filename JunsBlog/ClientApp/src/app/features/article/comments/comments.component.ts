import { Component, OnInit, Input } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommentService } from 'src/app/services/comment.service';
import { User } from 'src/app/models/user';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { CommentSearchPagingResult } from 'src/app/models/commentSearchPagingResult';
import { commentSearchOnEnum } from 'src/app/models/Enums/commentSearchOnEnum';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() article: ArticleDetails;
  currentUser: User;
 
 // currentPageResult: CommentPagingResult;

 commentPagingResult: CommentSearchPagingResult;
 defaultAvatarUrl = './assets/avatar.png';

 throttle = 300;
 scrollDistance = 1;
 scrollUpDistance = 2;
 loading = false;
 commentsCount = 0;

 constructor(private commentService: CommentService, private toastr: ToastrService) { }

 ngOnInit(): void {
   const intiPage: number = 1;
   const pageSize: number = 10;
   this.loading = true;
   this.commentService.searchComments(intiPage, pageSize, this.article.id, commentSearchOnEnum.TargetId).subscribe(x=>{
     this.article.comments = x.documents;
     this.commentPagingResult = x;
     this.commentsCount = x.totalDocuments;
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
 }


 onScrollDown () {
   console.log('hehhe');
   if(this.commentPagingResult && this.commentPagingResult.hasNextPage && !this.loading){
     this.loading = true;
     this.commentService.searchComments(this.commentPagingResult.currentPage + 1 , this.commentPagingResult.pageSize, 
      this.commentPagingResult.searchKey, this.commentPagingResult.searchOn, this.commentPagingResult.sortBy, this.commentPagingResult.sortOrder).subscribe(
       data => {        
           data.documents.forEach(doc => {
            this.article.comments .push(doc);
           });
           this.commentPagingResult = data;
           this.loading = false;
         },
       err => {
         this.loading = false;  
         this.toastr.error('Unknown error occurred, please try again later');
       }
     )
   }
 }
}
