import {inject, Injectable} from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import {tap} from 'rxjs';
import {CreateTaskRequest} from '../interfaces/requests/create-task-request';
import {UpdateTaskRequest} from '../interfaces/requests/update-task-request';
import { PagedData } from '../interfaces/models/paged-data';
import {Task} from '../interfaces/models/task';
import {environment} from '../environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class TaskService {
  httpClient = inject(HttpClient)
  private readonly apiUrl = `${environment.apiUrl}/api/tasks`

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
      this.apiUrl,
      {
        params,
        withCredentials: true
      }
    );
  }

  createTask(request: CreateTaskRequest) {
    return this.httpClient.post<void>(`${this.apiUrl}`, request, { withCredentials: true });
  }

  updateTask(taskId: number, request: UpdateTaskRequest) {
    return this.httpClient.put(`${this.apiUrl}/${taskId}`, request, { withCredentials: true });
  }

  deleteTask(taskId: number) {
    return this.httpClient.delete(`${this.apiUrl}/${taskId}`, { withCredentials: true });
  }
}
