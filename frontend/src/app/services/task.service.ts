import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {Task} from '../interfaces/task';
import {tap} from 'rxjs';
import {PagedData} from '../interfaces/paged-data';
import {CreateTaskRequest} from '../interfaces/create-task-request';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  httpClient = inject(HttpClient)
  baseApiUrl = 'http://localhost:5284/api/tasks'

  pagedTasks: PagedData<Task> | null = null;

  searchTasks(query: string | null, categoryId: number | null, page: number, pageSize: number) {

    let params = new HttpParams()
      .set('page', page)
      .set('pageSize', pageSize);

    if (query !== null) {
      params = params.set('searchQuery', query);
    }

    if (categoryId !== null) {
      params = params.set('categoryId', categoryId);
    }

    return this.httpClient.get<PagedData<Task>>(
      this.baseApiUrl,
      {
        params,
        withCredentials: true
      }
    ).pipe(
      tap(t => this.pagedTasks = t)
    );
  }

  createTask(request: CreateTaskRequest) {
    return this.httpClient.post<void>(`${this.baseApiUrl}`, request, { withCredentials: true });
  }
}
