import { Component, OnInit } from '@angular/core';
import { ArticleDetails } from 'src/app/models/article/articleDetails';
import { ActivatedRoute, Router } from '@angular/router';
import { ArticleService } from 'src/app/services/article.service';
import { mergeMap } from 'rxjs/operators';
import { AlertService } from 'src/app/services/alert.service';

@Component({
  selector: 'app-article-viewer',
  templateUrl: './article-viewer.component.html',
  styleUrls: ['./article-viewer.component.scss']
})
export class ArticleViewerComponent implements OnInit {

  article: ArticleDetails;
  isProcessing: boolean;
  loading: boolean;
  
  constructor(private route: ActivatedRoute,  private articleService: ArticleService, private alertService: AlertService,
    private router: Router) { }

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
        this.alertService.alertHttpError(err);
      }
    )
  }

  edit(){
    this.router.navigateByUrl(`/editor/${this.article.id}`);
  }

}
