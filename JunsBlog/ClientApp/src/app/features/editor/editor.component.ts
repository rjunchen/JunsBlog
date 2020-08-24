import { Component, OnInit } from '@angular/core';
import { Article } from 'src/app/models/article';
import { ArticleService } from 'src/app/services/article.service';
import {MatChipInputEvent} from '@angular/material/chips';
import {COMMA, ENTER} from '@angular/cdk/keycodes';

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
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];

  constructor(private articleService: ArticleService) { }

  ngOnInit(): void {
    this.article = new Article();
    this.article.categories = [];
  }

  cancel(){

  }

  summit(){
    this.articleService.CreateArticle(this.article).subscribe( x=>{

    }, err=>{

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

  remove(fruit: string): void {
    const index = this.article.categories.indexOf(fruit);

    if (index >= 0) {
      this.article.categories.splice(index, 1);
    }
  }

}
