import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/user';
import { AuthenticationService } from 'src/app/core/authentication.service';
import { CommentService } from 'src/app/services/comment.service';
import { CommentRequest } from 'src/app/models/commentRequest';
import { CommenterRequest } from 'src/app/models/commenterRequest';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-comment-control',
  templateUrl: './comment-control.component.html',
  styleUrls: ['./comment-control.component.scss']
})
export class CommentControlComponent implements OnInit {

  isVisibleCommenter: boolean;
  commentText = '';
  defaultAvatarUrl = './assets/avatar.png';
  currentUser: User;
  commenterRequest: CommenterRequest;

  constructor(private auth: AuthenticationService, private commentService: CommentService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.currentUser = this.auth.getCurrentUser();
    this.commentService.onShowCommentControl.subscribe( data =>{
      console.log(data);
      this.isVisibleCommenter = true;
      this.commenterRequest = data;
    });
  }

  submit(){
    var commentRequest = new CommentRequest();
    commentRequest.content = this.commentText;
    commentRequest.targetId = this.commenterRequest.callerId;
    this.commentService.replyArticle(commentRequest).subscribe(x=>{
      this.commenterRequest.comments.unshift(x);
    }, err=>{
      this.toastr.error('Unknown error occurred, please try again later');
    })
    this.isVisibleCommenter = false;
  }

  cancel(){
    this.isVisibleCommenter = false;
  }
}
