import {inject, Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {RegisterRequest} from '../interfaces/register-request';
import {User} from '../interfaces/user';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient)
  private baseApiUrl = 'http://localhost:5284/api/auth'
  user: User | undefined = undefined;

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
      tap(() => this.user = undefined)
    );
  }
}
