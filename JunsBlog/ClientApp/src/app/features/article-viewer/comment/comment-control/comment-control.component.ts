import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/authentication/user';
import { Subscription } from 'rxjs';
import { AuthenticationService } from 'src/app/services/authentication.service';
import { AlertService } from 'src/app/services/alert.service';
import { CommentService } from '../../../../services/comment.service'
import { CommentRequest } from 'src/app/models/comment/commentRequest';
import { Router } from '@angular/router';

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
  commentSubscription: Subscription;

  constructor(private auth: AuthenticationService,  private router: Router, private commentService: CommentService, private alertService: AlertService) { }
  ngOnDestroy(): void {
    this.commentSubscription.unsubscribe();
  }

  ngOnInit(): void {
    this.currentUser = this.auth.getCurrentUser();
   this.commentSubscription = this.commentService.onShowCommentControl.subscribe( data =>{
      if(!this.currentUser){
        this.router.navigateByUrl('login');
      }
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
      this.alertService.alertHttpError(err);
    })
    this.isVisibleCommenter = false;
  }

  cancel(){
    this.isVisibleCommenter = false;
  }

}
