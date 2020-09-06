import { Component } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'ClientApp';

  items$ = [];

  constructor(){
    for (let index = 0; index < 100; index++) {
      this.items$.push(index);
    }
  }
  
}
