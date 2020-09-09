import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { ArticleService } from 'src/app/services/article.service';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';
import { SortByEnum } from 'src/app/models/enums/sortByEnum';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, AfterViewInit {

  sortBy: string = 'recent';
  searchOption: ArticleSearchPagingOption;
  constructor(private route: ActivatedRoute, private articleService: ArticleService, private alertService: AlertService) { }
  ngAfterViewInit(): void {
    this.route.paramMap.subscribe(x=>{
      const searchKey = x.get('searchKey')
      this.searchOption = new ArticleSearchPagingOption();
      this.searchOption.searchKey = searchKey;
      setTimeout(() => {
        this.articleService.SearchClicked(this.searchOption);
      }, 1);
     });
  }

  ngOnInit(): void {
  
  }

  sortByChanged(){
    if(this.sortBy == 'views'){
      this.searchOption.sortBy = SortByEnum.Views;
    }
    else if(this.sortBy == 'likes') {
      this.searchOption.sortBy = SortByEnum.Likes;
    }
    else if(this.sortBy == 'favors') {
      this.searchOption.sortBy = SortByEnum.Favors;
    }
    else{
      this.searchOption.sortBy = SortByEnum.CreatedOn;
    }
    this.articleService.SearchClicked(this.searchOption);
  }
}
