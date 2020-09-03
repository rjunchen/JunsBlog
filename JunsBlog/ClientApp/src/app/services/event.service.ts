import { Injectable, Output, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EventService {
  @Output() onSidebarToggled: EventEmitter<any> = new EventEmitter();
  constructor() { }

  public toggleSidebar(){
    this.onSidebarToggled.emit();
  }
}

