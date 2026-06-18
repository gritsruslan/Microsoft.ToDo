import { HttpClient } from '@angular/common/http';
import {inject, Injectable } from '@angular/core';
import {tap} from 'rxjs';
import {Category} from '../interfaces/models/category';
import {environment} from '../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private httpClient = inject(HttpClient)
  private readonly apiUrl = `${environment.apiUrl}/api/categories`

  getCategories() {
    return this.httpClient.get<Category[]>(this.apiUrl, {withCredentials: true})
  }

  getCategoryById(id: number) {
    return this.httpClient.get<Category>(`${this.apiUrl}/${id}`, {withCredentials: true})
  }

  createCategory(name: string) {
    return this.httpClient.post<Category>(`${this.apiUrl}?name=${name}`, {}, {withCredentials: true})
  }
}
