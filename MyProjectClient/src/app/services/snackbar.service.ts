import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  constructor(private snackBar: MatSnackBar) {}

  showLoginSuccess(): void {
    this.snackBar.open('Login Successful', 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }

  showLoginRequiredMessage() {
    const config = new MatSnackBarConfig();
    config.duration = 4000;
    config.panelClass = ['snackbar-login-required'];
    config.verticalPosition = 'bottom';

    this.snackBar.open(
      'You must be logged in to access this lesson.',
      'Close',
      config
    );
  }

  showInfoSnackbar(message: string) {
    this.snackBar.open(message, 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }
}
