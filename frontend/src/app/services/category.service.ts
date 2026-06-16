import { HttpClient } from '@angular/common/http';
import {inject, Injectable } from '@angular/core';
import {Category} from '../interfaces/category';
import {tap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private httpClient = inject(HttpClient)
  private baseApiUrl = `http://localhost:5284/api/categories`

  categories: Category[] = [];

  getCategories() {
    return this.httpClient.get<Category[]>(this.baseApiUrl, {withCredentials: true})
    .pipe(
      tap(c => this.categories = c)
    )
  }

  createCategory(name: string) {
    return this.httpClient.post<Category>(`${this.baseApiUrl}?name=${name}`, {}, {withCredentials: true})
    .pipe(
      tap(newCategory => this.categories = [newCategory, ...this.categories])
    )
  }
}
