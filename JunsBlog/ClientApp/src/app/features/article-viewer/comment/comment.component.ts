import { Component, OnInit, Input } from '@angular/core';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { User } from 'src/app/models/authentication/user';
import { CommentService } from 'src/app/services/comment.service';
import { CommentRequest } from 'src/app/models/comment/commentRequest';

@Component({
  selector: 'app-comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent implements OnInit {
  @Input() article: ArticleDetails;
  currentUser: User;

  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  loading = false;
  displayComments: any[] = [];

  constructor(private commentService: CommentService) { }

  ngOnInit(): void {
  }

  onScrollDown (){

  }

  showCommenter(){
    var request = new CommentRequest(this.article);
    this.commentService.showCommentControl(request)
  }

}
