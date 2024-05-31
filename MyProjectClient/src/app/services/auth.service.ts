import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';

@Injectable()
export class AuthService {
  constructor(private httpClient: HttpClient) {}

  login(email: string, password: string) {
    return this.httpClient.post<any>(
      `${environment.apiUrl}/Login/Login`,
      {
        email,
        password,
      },
      { withCredentials: true }
    );
  }

  isLoggedIn() {
    return localStorage.getItem('currentUser') ? true : false;
  }

  logout() {
    localStorage.removeItem('currentUser');
  }
}
