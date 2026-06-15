import {inject, Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {RegisterRequest} from '../interfaces/register-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private httpClient = inject(HttpClient)
  private baseApiUrl = 'http://localhost:5284/api/auth'

  register(request: RegisterRequest) {
    return this.httpClient.post<void>(`${this.baseApiUrl}/register`, request)
  }

  login(request: RegisterRequest) {
    return this.httpClient.post<void>(`${this.baseApiUrl}/login`, request)
  }
}
