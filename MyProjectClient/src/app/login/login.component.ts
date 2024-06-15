import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  user: string = '';
  password: string = '';
  error: string = '';
  submited: boolean = false;
  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login(this.user, this.password).subscribe((response) => {
      this.submited = true;
      if (response?.token) {
        this.authService.setTokens(response);
        this.router.navigate(['/homepage']);
      } else {
        this.error = response.message;
      }
    });
  }
}
