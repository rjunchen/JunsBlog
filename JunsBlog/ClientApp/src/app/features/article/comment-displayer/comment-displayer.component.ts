import { Component, OnInit, Input } from '@angular/core';
import { CommentDetails } from 'src/app/models/commentDetails';
import { CommentService } from 'src/app/services/comment.service';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { ToastrService } from 'ngx-toastr';
import { CommenterRequest } from 'src/app/models/commenterRequest';
import { CommentTypeEnum } from 'src/app/models/Enums/commentTypeEnum';


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
  constructor(private commentService: CommentService, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  likeComment(comment: CommentDetails){

  }

  dislikeComment(comment: CommentDetails){

  }

  showCommentControl(comment: CommentDetails){
    var commenterRequest = new CommenterRequest(comment.id, comment.comments, CommentTypeEnum.Comment);
    this.commentService.showCommentControl(commenterRequest);
  }

  viewReplies(){
    this.viewRepliesOpen = !this.viewRepliesOpen;
        this.commentService.getComments(this.comment.id).subscribe(
          data => {
            data.forEach(doc => {
              if(!this.comment.comments) this.comment.comments = [];
              this.comment.comments.push(doc);
            });       
            this.loading = false;
        },
        err => {
          this.loading = false;
          this.toastr.error('Unknown error occurred, please try again later');
        }
      )
    
  }
}
