import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { CommentService } from 'src/app/services/comment.service';
import { ToastrService } from 'ngx-toastr';
import { CommentRequest } from 'src/app/models/comment/commentRequest';

@Component({
  selector: 'app-comment-control',
  templateUrl: './comment-control.component.html',
  styleUrls: ['./comment-control.component.scss']
})
export class CommentControlComponent implements OnInit {

  isVisibleCommenter: boolean;
  defaultAvatarUrl = './assets/avatar.png';
  currentUser: User;
  commentRequest: CommentRequest;

  constructor(private auth: AuthenticationService, private commentService: CommentService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.currentUser = this.auth.getCurrentUser();
    this.commentService.onShowCommentControl.subscribe( data =>{
      this.isVisibleCommenter = true;
      this.commentRequest = data;
    });
  }

  submit(){
    // prefix the @User if comment is reply to a child comment.
    if(this.commentRequest.replyToUser){
      this.commentRequest.commentText = `@${this.commentRequest.replyToUser.name} ${this.commentRequest.commentText}`;
    }

    this.commentService.replyArticle(this.commentRequest).subscribe(x=>{
      this.commentService.commentPosted(x);
      this.isVisibleCommenter = false;
    }, err=>{
      if (err.status != 400) {     
        this.toastr.warning(err.error.message, err.statusText);
      } else {
        this.toastr.error('Unknown error occurred, please try again later');
      }
    })
    this.isVisibleCommenter = false;
  }

  cancel(){
    this.isVisibleCommenter = false;
  }
}
