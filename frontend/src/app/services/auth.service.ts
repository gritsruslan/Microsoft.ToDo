import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {tap} from 'rxjs';
import {RegisterRequest} from '../interfaces/requests/login-request';
import {User} from '../interfaces/models/user';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient)
  private baseApiUrl = 'http://localhost:5284/api/auth'
  user: User | null = null;

  get isAuth() {
    return !!this.user;
  }

  getMe() {
    return this.httpClient.get<User>(`${this.baseApiUrl}/me`, {withCredentials: true})
    .pipe(
      tap(user => this.user = user)
    );
  }

  register(request: RegisterRequest) {
    return this.httpClient.post<void>(`${this.baseApiUrl}/register`, request)
  }

  login(request: RegisterRequest) {
    return this.httpClient.post<void>(`${this.baseApiUrl}/login`, request, {withCredentials: true})
  }

  logout() {
    return this.httpClient.post<void>(`${this.baseApiUrl}/logout`, {}, {withCredentials: true})
    .pipe(
      tap(() => this.user = null)
    );
  }
}
