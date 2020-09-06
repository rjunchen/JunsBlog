import { Component, OnInit } from '@angular/core';
import { AlertService } from '../../services/alert.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  
  throttle = 300;
  scrollDistance = 1;
  scrollUpDistance = 2;
  
  constructor(private alertService: AlertService) { }

  ngOnInit(): void {
  }

  warning(){
    this.alertService.warning('Tis is a testThis is a testThis is a testThis is a test', 'Ok');
  }

  info(){
    this.alertService.info('Tis is a testThis is a testThis is a testThis is a test', 'Ok', 0);
  }
  error(){
    this.alertService.error('Tis is a testThis is a testThis is a testThis is a test', 'Ok', 0, 'bottom', 'end');
  }

  success(){
    this.alertService.success('Tis is a testThis is a testThis is a testThis is a test', 'Ok', 0);
  }
  onScrollDown(){
    console.log('i am goog');
  }
}
