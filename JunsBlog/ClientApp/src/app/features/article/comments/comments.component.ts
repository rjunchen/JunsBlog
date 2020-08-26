import { Component, OnInit, Input } from '@angular/core';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';
import { CommentService } from 'src/app/services/comment.service';
import { User } from 'src/app/models/user';
import { CommentRequest } from 'src/app/models/commentRequest';
import { CommentDetails } from 'src/app/models/commentDetails';
import { Article } from 'src/app/models/article';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { CommenterRequest } from 'src/app/models/commenterRequest';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss']
})
export class CommentsComponent implements OnInit {
  @Input() article: ArticleDetails;
  currentUser: User;
 
 // currentPageResult: CommentPagingResult;

  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  loading = false;



  constructor(private commentService: CommentService, private toastr: ToastrService,
    private router: Router, private auth: AuthenticationService) { }

  ngOnInit(): void {
    this.commentService.getComments(this.article.id).subscribe(x=>{
      console.log(x);
      this.article.comments = x;

      this.article.comments.forEach(comment => {
        if(!comment.comments) comment.comments = []; 
      });

    }, err=>{

    })
  }

}
