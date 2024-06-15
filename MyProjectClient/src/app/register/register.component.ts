import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MustMatch } from '../utils/must-match.validator';
import { RegisterData } from '../models/user';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss',
})
export class RegisterComponent implements OnInit {
  registerForm: FormGroup = this.formBuilder.group({});
  submitted = false;
  statusMessage: string = '';

  data: RegisterData = {
    username: '',
    email: '',
    password: '',
  };

  constructor(
    private formBuilder: FormBuilder,
    private userService: UserService
  ) {}

  ngOnInit(): void {
    this.registerForm = this.formBuilder.group(
      {
        username: [
          this.data.username,
          [
            Validators.required,
            Validators.minLength(6),
            Validators.maxLength(20),
          ],
        ],
        email: [this.data.email, [Validators.required, Validators.email]],
        password: [
          this.data.email,
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

  onSubmit() {
    this.submitted = true;

    if (this.registerForm.invalid) return;

    this.data = this.registerForm.value;

    this.userService.InsertUser(this.data).subscribe((response) => {
      this.statusMessage = response.message;
    });
  }

  onReset() {
    this.submitted = false;
    this.registerForm.reset();
  }
}
