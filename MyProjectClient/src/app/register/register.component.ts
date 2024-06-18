import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MustMatch } from '../utils/must-match.validator';
import { RegisterData } from '../models/user';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
})
export class RegisterComponent implements OnInit {
  registerForm: any;
  submitted = false;
  statusMessage: string = '';

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.registerForm = this.formBuilder.group(
      {
        username: [
          '',
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
          ],
        ],
        email: ['', [Validators.required, Validators.email]],
        password: [
          '',
          [
            Validators.required,
            Validators.minLength(8),
            Validators.maxLength(32),
          ],
        ],
        confirmPassword: ['', Validators.required],
      },
      {
        validator: MustMatch('password', 'confirmPassword'),
      }
    );
  }

  get f() {
    return this.registerForm.controls;
  }

  onSubmit(): void {
    this.submitted = true;

    if (this.registerForm.invalid) {
      return;
    }

    const userData: RegisterData = this.registerForm.value;

    this.userService.insertUser(userData).subscribe(
      (response) => {
        this.statusMessage = response.message;
      },
      (error) => {
        this.statusMessage =
          'Error occurred during registration. Please try again.';
        console.error('Registration Error:', error);
      }
    );
  }

  onReset(): void {
    this.submitted = false;
    this.registerForm.reset();
    this.statusMessage = '';
  }
}
