import { HttpClient } from '@angular/common/http';
import {inject, Injectable } from '@angular/core';
import {tap} from 'rxjs';
import {Category} from '../interfaces/models/category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private httpClient = inject(HttpClient)
  private baseApiUrl = `http://localhost:5284/api/categories`

  getCategories() {
    return this.httpClient.get<Category[]>(this.baseApiUrl, {withCredentials: true})
  }

  getCategoryById(id: number) {
    return this.httpClient.get<Category>(`${this.baseApiUrl}/${id}`, {withCredentials: true})
  }

  createCategory(name: string) {
    return this.httpClient.post<Category>(`${this.baseApiUrl}?name=${name}`, {}, {withCredentials: true})
  }
}
