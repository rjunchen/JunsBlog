import { Component, OnInit, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { MatChipInputEvent, MatChipList } from '@angular/material/chips';
import { Article } from './../../models/article/article';
import { ArticleService } from './../../services/article.service';
import { COMMA, ENTER } from '@angular/cdk/keycodes';

import Quill from 'quill'
import ImageResize from 'quill-image-resize-module'
Quill.register('modules/imageResize', ImageResize)

import { AlertService } from 'src/app/services/alert.service';
import { FormGroup, FormBuilder, Validators, FormControl } from '@angular/forms';


export interface Category {
  name: string;
}

@Component({
  selector: 'app-article-editor',
  templateUrl: './article-editor.component.html',
  styleUrls: ['./article-editor.component.scss']
})
export class ArticleEditorComponent implements OnInit {
  @ViewChild('chipList') chipList: MatChipList;
  modules = {}

  article: Article;
  articleForm: FormGroup;
  visible = true;
  selectable = true;
  removable = true;
  addOnBlur = true;
  updateMode = false;
  readonly separatorKeysCodes: number[] = [ENTER, COMMA];
  deepCopyCategories= [];
  isProcessing: boolean;

  constructor(private articleService: ArticleService, private router: Router, private route: ActivatedRoute,
    private alertService: AlertService, private fb: FormBuilder) {
    this.modules = {
      imageResize: {},
      toolbar: [
          ['bold', 'italic', 'underline', 'strike'],
          [{ 'list': 'ordered'}, { 'list': 'bullet' }],
          [{ 'color': [] }], 
          ['link', 'image', 'video'] 
        ]
    };
   }

  ngOnInit(): void {
    
    this.route.paramMap.subscribe(x=>{
      var articleId = x.get("id")
      if(articleId){
        // Update mode
        this.updateMode = true;
        this.isProcessing = true;
        this.articleService.getArticle(articleId).subscribe( data=> {
          this.isProcessing = false;
          this.article = data;
          this.createForm();
        }, err=>{
          this.isProcessing = false;
          this.alertService.alertHttpError(err);
        })
      }else{
        // create mode
        this.article = new Article();
        this.createForm();
      }
    });
  }

  createForm() {
    this.deepCopyCategories = JSON.parse(JSON.stringify(this.article.categories));
    this.articleForm = this.fb.group({
      title: [ this.article.title, Validators.required],
      abstract: [this.article.abstract, Validators.required],
      content: [this.article.content, Validators.required],
      isPrivate: [this.article.isPrivate, Validators.required],
    })
  }

  onBlurCategories(){
    if(this.deepCopyCategories.length > 0)
    {
      this.chipList.errorState = false;
    }   
    else
    {
      this.chipList.errorState = true;
    }
  }

  cancel(){
    this.createForm();
    this.chipList.errorState = false;
  }

  get articleFormControl() {
    return this.articleForm.controls;
  }
  
  canSubmit(){
    return this.articleFormControl.title.valid && this.articleFormControl.abstract.valid 
    && this.articleFormControl.content.valid && this.deepCopyCategories.length > 0;
  }

  summit(){
    this.article.title = this.articleFormControl.title.value;
    this.article.abstract = this.articleFormControl.abstract.value;
    this.article.content = this.articleFormControl.content.value;
    this.article.categories = this.deepCopyCategories;
    this.article.isPrivate = this.articleFormControl.isPrivate.value;
    this.isProcessing = true;
    this.articleService.saveArticle(this.article).subscribe( article=>{
      this.isProcessing = false;
      this.router.navigateByUrl(`/article/${article.id}`);
    }, err=>{
      this.isProcessing = false;
       this.alertService.alertHttpError(err);
    })
  }

  add(event: MatChipInputEvent): void {
    const input = event.input;
    const value = event.value;

    // Add category
    if ((value || '').trim()) {
      this.deepCopyCategories.push(value.trim());
      this.chipList.errorState = false;
    }

    // Reset the input value
    if (input) {
      input.value = '';
    }
  }

  remove(category: string): void {
    const index = this.deepCopyCategories.indexOf(category);
    if (index >= 0) {
      this.deepCopyCategories.splice(index, 1);
    }
  }

  // @HostListener('window:beforeunload', ['$event'])
  // unloadHandler(event: Event) {
  //   // Your logic on beforeunload
  // }

}
