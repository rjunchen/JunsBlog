import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { Article } from 'src/app/models/article/article';
import { ArticleService } from 'src/app/services/article.service';
import { ToastrService } from 'ngx-toastr';
import { ArticleSearchPagingResult } from 'src/app/models/article/articleSearchPagingResult';
import { ArticleSearchPagingOption } from 'src/app/models/article/articleSearchPagingOption';
import { Subscription } from 'rxjs';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery-9';
import { GalleryImage } from 'src/app/models/article/galleryImage';

@Component({
  selector: 'app-articles-list',
  templateUrl: './articles-list.component.html',
  styleUrls: ['./articles-list.component.scss']
})
export class ArticlesListComponent implements OnInit, OnDestroy {
  @Input() loadOnInit: boolean;

  articles: Article[];
  articlePagingResult: ArticleSearchPagingResult;
  defaultAvatarUrl = './assets/avatar.png';

  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  loading = false;
  noSearchResult: boolean;
  searchSubscription: Subscription;

  galleryOptions: NgxGalleryOptions[];

  constructor(private articleService: ArticleService, private toastr: ToastrService) { }
  ngOnDestroy(): void {
    this.searchSubscription.unsubscribe();  //Leave the page doesn't unsubscribe by default, need manually unsubscribe it
  }

  ngOnInit(): void {
    this.searchSubscription = this.articleService.onSearchClicked.subscribe(option=>{
      this.search(option);
    })

    if(this.loadOnInit){
      this.search(new ArticleSearchPagingOption());
    }
     this.previewImageSetup();
  }

  search(option: ArticleSearchPagingOption){
    this.loading = true;
    this.articleService.searchArticle(option).subscribe(x=>{
      
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

  previewImageSetup(){
    this.galleryOptions = [
            {
                // width: '900px',
                height: '200px',
                image: false,
                thumbnailsMargin: 2,
                thumbnailMargin: 2,
                thumbnailsRemainingCount: true,
                thumbnailsColumns: 3,
                thumbnailsRows: 1, 
                imageAnimation: NgxGalleryAnimation.Slide
            },
            // max-width 800
            {
                breakpoint: 768,
                width: '100%',
                height: '100px',
                imagePercent: 80,
                thumbnailsPercent: 20,
                thumbnailsRows: 1, 
                thumbnailsMargin: 2,
                thumbnailMargin: 2
            },
            // max-width 400
            {
                breakpoint: 400,
                preview: false
            }
        ];
  }

}
