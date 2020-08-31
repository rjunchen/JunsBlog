import { Component, OnInit, HostListener } from '@angular/core';
import { Article } from 'src/app/models/article/article';
import { ArticleService } from 'src/app/services/article.service';
import { MatChipInputEvent } from '@angular/material/chips';
import { COMMA, ENTER } from '@angular/cdk/keycodes';
import { ToastrService } from 'ngx-toastr';
import { Router, ActivatedRoute } from '@angular/router';
import { mergeMap } from 'rxjs/operators';

export interface Category {
  name: string;
}

@Component({
  selector: 'app-editor',
  templateUrl: './editor.component.html',
  styleUrls: ['./editor.component.scss']
})
export class EditorComponent implements OnInit {

  article: Article;

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  updateMode = false;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(private articleService: ArticleService, private toastr: ToastrService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    
    this.route.paramMap.subscribe(x=>{
      var articleId = x.get("id")
      if(articleId){
        // Update mode
        this.updateMode = true;
        this.articleService.getArticle(articleId).subscribe( ar=> {
          this.article = ar;
        }, err=>{
          if (err.status === 400) {     
            this.toastr.warning(err.error.message, err.statusText);
          } else {
            this.toastr.error('Unknown error occurred, please try again later');
          }
        })
      }else{
        // create mode
        this.article = new Article();
      }
    });
  }

  cancel(){

  }

  summit(){
    if(this.updateMode){
      this.articleService.UpdateArticle(this.article).subscribe( x=>{
        this.router.navigateByUrl(`/article/${x.id}`);
      }, err=>{
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      })
    }else{
      this.articleService.createArticle(this.article).subscribe( x=>{
        this.router.navigateByUrl(`/article/${x.id}`);
      }, err=>{
        if (err.status === 400) {     
          this.toastr.warning(err.error.message, err.statusText);
        } else {
          this.toastr.error('Unknown error occurred, please try again later');
        }
      })
    }
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add category
    if ((value || '').trim()) {
      this.article.categories.push(value.trim());
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  remove(category: string): void {
    const index = this.article.categories.indexOf(category);
    if (index >= 0) {
      this.article.categories.splice(index, 1);
    }
  }

  @HostListener('window:beforeunload', ['$event'])
  unloadHandler(event: Event) {
    // Your logic on beforeunload
  }

}
