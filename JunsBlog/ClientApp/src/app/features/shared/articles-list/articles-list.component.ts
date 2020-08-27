import { Component, OnInit, Input } from '@angular/core';
import { ArticleListConfig } from 'src/app/models/articleListConfig';
import { Article } from 'src/app/models/article';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ArticleSearchPagingResult } from 'src/app/models/articleSearchPagingResult';

@Component({
  selector: 'app-articles-list',
  templateUrl: './articles-list.component.html',
  styleUrls: ['./articles-list.component.scss']
})
export class ArticlesListComponent implements OnInit {
  @Input() articleListConfig: ArticleListConfig;

  articles: Article[];
  articlePagingResult: ArticleSearchPagingResult;
  defaultAvatarUrl = './assets/avatar.png';

  
  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  loading = false;

  constructor(private articleService: ArticleService, private toastr: ToastrService) { }

  ngOnInit(): void {
    const intiPage: number = 1;
    const pageSize: number = 10;
    this.loading = true;
    this.articleService.searchArticle(intiPage, pageSize).subscribe(x=>{
      this.articles = x.documents;
      this.articlePagingResult = x;
      this.loading = false;
    }, err =>{
      this.loading = false;
      if(err.status === 400){
        this.toastr.warning(err.error.message, err.statusText);
      }
      else{
        this.toastr.error('Unknown error occurred, please try again later');
      }
    });
  }


  onScrollDown () {
    console.log('down');
    if(this.articlePagingResult && this.articlePagingResult.hasNextPage && !this.loading){
      this.loading = true;
      this.articleService.searchArticle(this.articlePagingResult.currentPage + 1 , this.articlePagingResult.pageSize).subscribe(
        data => {        
            data.documents.forEach(doc => {
              this.articles.push(doc);
            });
            this.articlePagingResult = data;
            this.loading = false;
          },
        err => {
          this.loading = false;  
          this.toastr.error('Unknown error occurred, please try again later');
        }
      )
    }
  }

}
