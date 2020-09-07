import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatChipInputEvent } from '@angular/material/chips';
import { Article } from './../../models/article/article';
import { ArticleService } from './../../services/article.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';

import Quill from 'quill'
import ImageResize from 'quill-image-resize-module'
import { AlertService } from 'src/app/services/alert.service';
Quill.register('modules/imageResize', ImageResize)

export interface Category {
  name: string;
}

@Component({
  selector: 'app-article-editor',
  templateUrl: './article-editor.component.html',
  styleUrls: ['./article-editor.component.scss']
})
export class ArticleEditorComponent implements OnInit {

  modules = {}

  article: Article;

  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  updateMode = false;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(private articleService: ArticleService, private router: Router, private route: ActivatedRoute,
    private alertService: AlertService) {
    this.modules = {
      imageResize: {},
      toolbar: [
          ['bold', 'italic', 'underline', 'strike'],
          [{ 'list': 'ordered'}, { 'list': 'bullet' }],
          [{ 'color': [] }], 
          ['link', 'image', 'video'] 
        ]
    }
   }

  ngOnInit(): void {
    
    this.route.paramMap.subscribe(x=>{
      var articleId = x.get("id")
      if(articleId){
        // Update mode
        this.updateMode = true;
        this.articleService.getArticle(articleId).subscribe( data=> {
          this.article = data;
        }, err=>{
          this.alertService.alertHttpError(err);
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
    this.articleService.saveArticle(this.article).subscribe( id=>{
      this.router.navigateByUrl(`/article/${id}`);
    }, err=>{
       this.alertService.alertHttpError(err);
    })
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

  // @HostListener('window:beforeunload', ['$event'])
  // unloadHandler(event: Event) {
  //   // Your logic on beforeunload
  // }

}
