import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { UserService } from '../services/user.service';
import { HttpErrorResponse } from '@angular/common/http';
import { SnackbarService } from '../services/snackbar.service';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.scss'],
})
export class ProfileComponent implements OnInit {
  user: any;
  isEditingProfile: boolean = false;
  isChangingPassword: boolean = false;
  editUser: any;
  passwordData: any;

  constructor(
    private authService: AuthService,
    private userService: UserService,
    private snackbarService: SnackbarService
  ) {}

  ngOnInit(): void {
    const userDetails = this.authService.getUserDetailsFromToken();

    this.user = { ...userDetails };

    this.editUser = { ...this.user };
    this.passwordData = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    };
  }

  editProfile() {
    this.isChangingPassword = false;

    this.isEditingProfile = true;
    this.editUser = { ...this.user };
  }

  saveProfile() {
    if (this.editUser.username && this.editUser.email) {
      this.userService.updateUser(this.editUser).subscribe(
        (response: any) => {
          if (response.updated) {
            this.user = { ...this.editUser };
            this.isEditingProfile = false;
            this.snackbarService.showUpdateSuccess();
          } else {
            console.error('Update failed:', response.message);
          }
        },
        (error: HttpErrorResponse) => {
          this.snackbarService.showInfoSnackbar(
            'An error occured while updating profile.'
          );
        }
      );
    } else {
      alert('Please fill in all required fields.');
    }
  }

  cancelEdit() {
    this.isEditingProfile = false;
    this.editUser = { ...this.user };
  }

  changePassword() {
    // Close the Edit Profile form if it's open
    this.isEditingProfile = false;

    // Open the Change Password form
    this.isChangingPassword = true;
    this.passwordData = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    };
  }

  updatePassword() {
    if (
      this.passwordData.newPassword === this.passwordData.confirmPassword &&
      this.passwordData.currentPassword
    ) {
      this.authService
        .login(this.user.username, this.passwordData.newPassword)
        .subscribe((response) => {
          if (response?.token) {
            this.snackbarService.showPasswordChangeSucces();
          } else {
            alert('Error when changeing the password.');
          }
        });
      this.isChangingPassword = false;
    } else {
      alert('Please check that your passwords match and fill in all fields.');
    }
  }

  cancelPasswordChange() {
    this.isChangingPassword = false;
    this.passwordData = {
      currentPassword: '',
      newPassword: '',
      confirmPassword: '',
    };
  }
}
