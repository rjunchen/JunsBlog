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
      this.commenterRequest.commentDraft += "1";
    });
  }

  submit(){
    var commentRequest = new CommentRequest(this.commenterRequest);
    this.commentService.replyArticle(commentRequest).subscribe(x=>{
      this.commenterRequest.comments.unshift(x);
    }, err=>{
      if (err.status === 400) {     
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
