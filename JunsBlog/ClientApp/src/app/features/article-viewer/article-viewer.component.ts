import { Component, OnInit } from '@angular/core';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { ActivatedRoute } from '@angular/router';
import { ArticleService } from 'src/app/services/article.service';
import { mergeMap } from 'rxjs/operators';

@Component({
  selector: 'app-article-viewer',
  templateUrl: './article-viewer.component.html',
  styleUrls: ['./article-viewer.component.scss']
})
export class ArticleViewerComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;
  loading: boolean;
  
  constructor(private route: ActivatedRoute,  private articleService: ArticleService) { }

  ngOnInit(): void {
    this.loading = true;
    this.route.params.pipe(mergeMap(params => this.articleService.getArticleDetails(params['id']))
    ).subscribe(
      data => { 
        this.article = data;
        this.loading = false;
      },
      err => {
        this.loading = false;
        // if (err.status === 400) {     
        //   this.toastr.warning(err.error.message, err.statusText);
        // } else {
        //   this.toastr.error('Unknown error occurred, please try again later');
        // }
      }
    )
  }

}
