import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  let isLoggedIn = inject(AuthService).isLoggedIn();
  if (isLoggedIn) {
    return true;
  } else {
    inject(Router).navigate(['/login']);
    return false;
  }
};
