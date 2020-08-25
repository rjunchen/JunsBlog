import { Component, OnInit } from '@angular/core';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleDetails } from 'src/app/models/articleDetails';
import { mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-article',
  templateUrl: './article.component.html',
  styleUrls: ['./article.component.scss']
})
export class ArticleComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;

  constructor(private route: ActivatedRoute,  private articleService: ArticleService,
     private router: Router, private toastr: ToastrService) {}


  ngOnInit(): void {
    this.route.params.pipe(mergeMap(params => this.articleService.getArticle(params['id']))
    ).subscribe(
      data => { 
        console.log(data);
        this.article = data;
      },
      err => {
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      }
    )
  }

  like(){
  
  }

  favor(){
  
  }

  dislike(){
  }
}
