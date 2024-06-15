import { Component } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  buttonClicked: boolean = false;
  constructor(
    private authService: AuthService,
    private router: Router,
    private userService: UserService
  ) {}
  logout() {
    this.authService.logout();
    location.reload();
  }

  getAllUsers() {
    this.userService.GetAllUsers().subscribe((response) => {
      console.log(response);
    });
  }

  onClick() {
    this.buttonClicked = true;
  }
}
