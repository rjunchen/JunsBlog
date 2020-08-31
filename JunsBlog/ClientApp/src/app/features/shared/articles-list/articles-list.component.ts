import { Component, OnInit, Input } from '@angular/core';
import { Article } from 'src/app/models/article/article';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ArticleSearchPagingResult } from 'src/app/models/article/articleSearchPagingResult';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';

@Component({
  selector: 'app-articles-list',
  templateUrl: './articles-list.component.html',
  styleUrls: ['./articles-list.component.scss']
})
export class ArticlesListComponent implements OnInit {
  @Input() loadOnInit: boolean;

  articles: Article[];
  articlePagingResult: ArticleSearchPagingResult;
  defaultAvatarUrl = './assets/avatar.png';

  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  loading = false;
  noSearchResult: boolean;

  constructor(private articleService: ArticleService, private toastr: ToastrService) { }

  ngOnInit(): void {

    this.articleService.onSearchClicked.subscribe(option=>{
      this.search(option);
    })

    if(this.loadOnInit){
      this.search(new ArticleSearchPagingOption());
    }
  }

  search(option: ArticleSearchPagingOption){
    this.loading = true;
    this.articleService.searchArticle(option).subscribe(x=>{
      console.log(x);
      this.articles = x.documents;
      this.articlePagingResult = x;
      this.loading = false;
      this.noSearchResult = x.documents.length == 0;
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
    if(this.articlePagingResult && this.articlePagingResult.hasNextPage && !this.loading){
      this.loading = true;
      this.articlePagingResult.searchOption.currentPage += 1;
      this.articleService.searchArticle(this.articlePagingResult.searchOption).subscribe(
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
