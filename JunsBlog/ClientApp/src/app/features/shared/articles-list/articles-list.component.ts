import { Component, OnInit, Input } from '@angular/core';
import { ArticleListConfig } from 'src/app/models/articleListConfig';
import { Article } from 'src/app/models/article';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ArticlePagingResult } from 'src/app/models/articlePagingResult';

@Component({
  selector: 'app-articles-list',
  templateUrl: './articles-list.component.html',
  styleUrls: ['./articles-list.component.scss']
})
export class ArticlesListComponent implements OnInit {
  @Input() articleListConfig: ArticleListConfig;

  articles: Article[];
  articlePagingResult: ArticlePagingResult;
  defaultAvatarUrl = './assets/avatar.png';
  constructor(private articleService: ArticleService, private toastr: ToastrService) { }

  ngOnInit(): void {
    const intiPage: number = 1;
    const pageSize: number = 10;

    this.articleService.searchArticle(intiPage, pageSize).subscribe(x=>{
      console.log(x);
      this.articles = x.documents;
      this.articlePagingResult = x;
    }, err =>{
      if(err.status === 400){
        this.toastr.warning(err.error.message, err.statusText);
      }
      else{
        this.toastr.error('Unknown error occurred, please try again later');
      }
    });
  }
}
