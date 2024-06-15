import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';

export const authGuard: CanActivateFn = (route, state) => {
  let token = inject(AuthService).getToken();

  if (token) return true;

  inject(Router).navigate(['/login']);
  return false;
};
