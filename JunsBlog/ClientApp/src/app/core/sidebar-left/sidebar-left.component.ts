import { Component, OnInit, OnDestroy, ChangeDetectorRef } from '@angular/core';
import {MediaMatcher} from '@angular/cdk/layout';
import { EventService } from 'src/app/services/event.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-sidebar-left',
  templateUrl: './sidebar-left.component.html',
  styleUrls: ['./sidebar-left.component.scss']
})
export class SidebarLeftComponent implements OnInit, OnDestroy {

  opened: boolean;
  sidebarToggleSub: Subscription;
  mobileQuery: MediaQueryList;

  private _mobileQueryListener: () => void;

  constructor(private eventService: EventService, changeDetectorRef: ChangeDetectorRef, media: MediaMatcher) { 
    this.mobileQuery = media.matchMedia('(max-width: 768px)');
    this._mobileQueryListener = () => changeDetectorRef.detectChanges();
    this.mobileQuery.addListener(this._mobileQueryListener);
  }

  ngOnDestroy(): void {
    this.sidebarToggleSub.unsubscribe();
    this.mobileQuery.removeListener(this._mobileQueryListener);
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
