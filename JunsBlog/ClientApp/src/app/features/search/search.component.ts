import { Component, OnInit } from '@angular/core';
import { ArticleService } from 'src/app/services/article.service';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  initialCurrentPage: number = 1;
  pageSize: number = 3;
  isProcessing: boolean;
  searchOption: ArticleSearchPagingOption;
  constructor(private articleService: ArticleService) { }

  ngOnInit(): void {
    this.searchOption = new ArticleSearchPagingOption();
  }

  clear(){
    this.searchOption.searchKey = null;
  }
  search(){
    this.articleService.SearchClicked(this.searchOption);
  }
}
