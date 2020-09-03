import { Component, OnInit } from '@angular/core';
import { EventService } from '../../services/event.service'

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit {

  currentUser: any;

  constructor(private eventService: EventService) { }

  ngOnInit(): void {
  }

  toggleSidebar(){
    this.eventService.toggleSidebar();
  }
}
