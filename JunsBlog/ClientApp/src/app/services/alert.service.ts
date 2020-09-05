import { Injectable } from '@angular/core';
import {MatSnackBar, MatSnackBarConfig,  MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition, TextOnlySnackBar,
  MatSnackBarRef,} from '@angular/material/snack-bar';

 enum AlertType{
  Success,
  Info,
  Warning,
  Error
}

@Injectable({
  providedIn: 'root'
})
export class AlertService {

  constructor(private snackBar: MatSnackBar ) { }


  info(message: string, action: string  = 'OK', duration: number = 5000, 
    verticalPosition: MatSnackBarVerticalPosition = 'bottom',
    horizontalPosition: MatSnackBarHorizontalPosition = 'center'): MatSnackBarRef<TextOnlySnackBar>{

    return this.alert(message, action, duration, verticalPosition, horizontalPosition, AlertType.Info);
  }

  warning(message: string, action: string  = 'OK', duration: number = 8000, 
    verticalPosition: MatSnackBarVerticalPosition = 'bottom',
    horizontalPosition: MatSnackBarHorizontalPosition = 'center'): MatSnackBarRef<TextOnlySnackBar>{

    return this.alert(message, action, duration, verticalPosition, horizontalPosition, AlertType.Warning);
  }

  error(message: string, action: string  = 'OK', duration: number = 100000, 
    verticalPosition: MatSnackBarVerticalPosition = 'bottom',
    horizontalPosition: MatSnackBarHorizontalPosition = 'center'): MatSnackBarRef<TextOnlySnackBar>{

    return this.alert(message, action, duration, verticalPosition, horizontalPosition, AlertType.Error);
  }

  success(message: string, action: string = 'OK', duration: number = 5000, 
    verticalPosition: MatSnackBarVerticalPosition = 'bottom',
    horizontalPosition: MatSnackBarHorizontalPosition = 'center'): MatSnackBarRef<TextOnlySnackBar>{

    return this.alert(message, action, duration, verticalPosition, horizontalPosition, AlertType.Success);
  }
  
  alertHttpError(err: any){
    if (err.status === 400) {     
      this.warning(err.error.message);
    } else {
      this.error('Unknown error occurred, please try again later');
    }
  }

  private alert(message: string, action: string, duration: number, 
    verticalPosition: MatSnackBarVerticalPosition,
    horizontalPosition: MatSnackBarHorizontalPosition, alertType: AlertType): MatSnackBarRef<TextOnlySnackBar>{
    let config = new MatSnackBarConfig();
    config.verticalPosition = verticalPosition;
    config.horizontalPosition = horizontalPosition;
    config.duration = duration;
    switch (alertType) {
      case AlertType.Success:
          config.panelClass = ['alert-success'];
        break;
        case AlertType.Warning:
          config.panelClass = ['alert-warning'];
        break;
        case AlertType.Error:
          config.panelClass = ['alert-error'];
        break;
    }
    return this.snackBar.open(message, action, config);
  }


}
