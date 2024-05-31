import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  error: string = '';
  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.authService.login(this.email, this.password).subscribe((response) => {
      if (response) {
        localStorage.setItem('currentUser', JSON.stringify(response));
        console.log(localStorage);
        this.router.navigate(['/']);
      } else {
        this.error = 'Login unsuccessfull!';
      }
    });
  }
}
