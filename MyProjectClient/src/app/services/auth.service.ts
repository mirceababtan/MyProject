import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../environment/environment';

@Injectable()
export class AuthService {
  constructor(private httpClient: HttpClient) {}

  login(user: string, password: string) {
    return this.httpClient.post<any>(
      `${environment.apiUrl}/Auth/Login`,
      {
        user,
        password,
      },
      { withCredentials: true }
    );
  }

  setTokens(response: any): void {
    localStorage.setItem('token', response.token);
    localStorage.setItem('refreshToken', response.refreshToken);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRefreshToken(): string {
    return localStorage.getItem('refreshToken') || '';
  }

  getUserDetailsFromToken(): any {
    const token = this.getToken();
    if (token) {
      const payload = JSON.parse(atob(token.split('.')[1]));
      return {
        id: payload.id,
        username: payload.username,
        email: payload.email,
        memberSince: payload.memberSince,
      };
    }
    return null;
  }

  logout(): void {
    this.httpClient
      .delete<any>(`${environment.apiUrl}/Auth/Logout`, {
        params: { refreshToken: this.getRefreshToken() },
      })
      .subscribe();

    localStorage.removeItem('token');
    localStorage.removeItem('refreshToken');
  }
}
