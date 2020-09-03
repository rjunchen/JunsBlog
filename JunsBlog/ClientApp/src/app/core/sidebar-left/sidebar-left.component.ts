import { Component, OnInit, OnDestroy } from '@angular/core';
import { EventService } from 'src/app/services/event.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-sidebar-left',
  templateUrl: './sidebar-left.component.html',
  styleUrls: ['./sidebar-left.component.scss']
})
export class SidebarLeftComponent implements OnInit, OnDestroy {

  opened: boolean = true;
  sidebarToggleSub: Subscription;

  constructor(private eventService: EventService) { }

  ngOnDestroy(): void {
    this.sidebarToggleSub.unsubscribe();
  }

  ngOnInit(): void {
    this.sidebarToggleSub = this.eventService.onSidebarToggled.subscribe(x=>{
      this.opened = !this.opened;
    })
  }

  toggleSidebar(){
    this.opened = !this.opened;
  }

}
