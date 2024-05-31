import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { RegisterData, User } from '../models/user';
import { environment } from '../../environment/environment';

@Injectable()
export class UserService {
  constructor(private httpClient: HttpClient) {}

  GetUser(): Observable<User> {
    return this.httpClient.get<User>(`${environment.apiUrl}/User`, {
      withCredentials: true,
    });
  }

  GetUserByEmail(email: string): Observable<User> {
    return this.httpClient.get<User>(`${environment.apiUrl}/GetUserByEmail`);
  }

  InsertUser(data: RegisterData) {
    return this.httpClient.post<any>(
      `${environment.apiUrl}/User/Register`,
      data,
      { withCredentials: true }
    );
  }
}
