import { TestBed } from '@angular/core/testing';

import { AlertService } from './alert.service';
import { MatSnackBarModule } from '@angular/material/snack-bar';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

describe('AlertService', () => {
  let service: AlertService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        MatSnackBarModule, BrowserAnimationsModule
      ],
    });
    service = TestBed.inject(AlertService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should be alert error', () => {
    service.error('this is error message');
    const errorDiv = document.querySelector('.alert-error');
    expect(errorDiv).toBeTruthy();
  });

  it('should be alert warning', () => {
    service.warning('this is warning message');
    const errorDiv = document.querySelector('.alert-warning');
    expect(errorDiv).toBeTruthy();
  });

  it('should be alert success', () => {
    service.success('this is success message');
    const errorDiv = document.querySelector('.alert-success');
    expect(errorDiv).toBeTruthy();
  });

});
