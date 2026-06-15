import {inject, Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {RegisterRequest} from '../interfaces/register-request';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  httpClient = inject(HttpClient)

  baseApiUrl = 'http://localhost:5284/api/auth'

  register(request: RegisterRequest) {
    return this.httpClient.post<void>(`${this.baseApiUrl}/register`, request)
  }
}
