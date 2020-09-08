import { Component, OnInit, AfterViewInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from 'src/app/services/alert.service';
import { ArticleService } from 'src/app/services/article.service';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit, AfterViewInit {

  constructor(private route: ActivatedRoute, private articleService: ArticleService, private alertService: AlertService) { }
  ngAfterViewInit(): void {
    this.route.paramMap.subscribe(x=>{
      const searchKey = x.get("searchKey")
      let searchOption = new ArticleSearchPagingOption();
      searchOption.searchKey = searchKey;
      this.articleService.SearchClicked(searchOption);
     });
  }

  ngOnInit(): void {
  
  }

}
