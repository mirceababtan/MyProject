import { Injectable } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class SnackbarService {
  showSuccessMessage(arg0: string) {
    this.snackBar.open('Course deleted successfully!', 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }
  showCourseSuccess() {
    this.snackBar.open('Course added successfully!', 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }
  constructor(private snackBar: MatSnackBar) {}

  showLoginSuccess(): void {
    this.snackBar.open('Login Successful', 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }

  showPasswordChangeSucces(): void {
    this.snackBar.open('Password changed succesfully.', 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }

  showEnrollRequiredMessage() {
    const config = new MatSnackBarConfig();
    config.duration = 4000;
    config.panelClass = ['snackbar-login-required'];
    config.verticalPosition = 'bottom';

    this.snackBar.open(
      'You must be enrolled in this course to  access this lesson.',
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

  showUpdateSuccess(): void {
    this.snackBar.open('Profile updated successfully!', 'Close', {
      duration: 3000,
      verticalPosition: 'bottom',
      horizontalPosition: 'center',
    });
  }
}
