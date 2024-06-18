import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  user: string = '';
  password: string = '';
  error: string = '';
  submitted: boolean = false;
  constructor(
    private authService: AuthService,
    private router: Router,
    private snackbarService: SnackbarService
  ) {}

  login() {
    this.authService.login(this.user, this.password).subscribe((response) => {
      this.submitted = true;
      if (response?.token) {
        this.authService.setTokens(response);
        this.snackbarService.showLoginSuccess();
        this.router.navigate(['/dashboard']);
      } else {
        this.error = response.message;
      }
    });
  }
}
