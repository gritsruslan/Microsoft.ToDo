import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../../services/task.service';

import { Task } from '../../interfaces/task';
import { PagedData } from '../../interfaces/paged-data';

import { PaginationComponent } from '../../common-ui/pagination/pagination.component';
import {TaskListComponent} from '../../common-ui/task-list/task-list.component';
import {EditTaskModalComponent} from '../../common-ui/edit-task-modal/edit-task-modal.component';
import {SearchFormComponent} from './search-form/search-form.component';

@Component({
  selector: 'app-search-page',
  imports: [
    TaskListComponent,
    PaginationComponent,
    EditTaskModalComponent,
    SearchFormComponent
  ],
  templateUrl: './search-tasks-page.component.html'
})
export class SearchPageComponent {

  private taskService = inject(TaskService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  pagedTasks: PagedData<Task> | null = null;

  currentPage = 1;
  pageSize = 7;

  query = '';

  isLoading = false;
  isError = false;

  selectedTask: Task | null = null;
  isEditOpen = false;

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      this.query = params.get('query') ?? '';
      this.currentPage = Number(params.get('page') ?? 1);

      this.search();
    });
  }

  search() {
    this.isLoading = true;
    this.isError = false;

    this.taskService
    .searchTasks(this.query, null, this.currentPage, this.pageSize)
    .subscribe({
      next: data => {
        this.pagedTasks = data;
        this.isLoading = false;
      },
      error: () => {
        this.isError = true;
        this.isLoading = false;
      }
    });
  }

  onSearch(query: string) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        query,
        page: 1
      }
    });
  }

  onDelete(taskId: number) {
    this.taskService.deleteTask(taskId)
    .subscribe(() => {
      this.search();
    });
  }

  onPageChange(page: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        page
      },
      queryParamsHandling: 'merge'
    });
  }

  onToggle(task: Task) {
    this.taskService.updateTask(task.id, {
      title: task.title,
      dueDate: task.dueDate,
      isCompleted: !task.isCompleted,
      categoryId: task.categoryId
    })
    .subscribe(() => {
      this.search();
    });
  }

  onEdit(task: Task) {
    this.selectedTask = task;
    this.isEditOpen = true;
  }

  closeEdit() {
    this.selectedTask = null;
    this.isEditOpen = false;
  }

  onSaveTask(updated: Task) {
    this.taskService.updateTask(updated.id, {
      title: updated.title,
      dueDate: updated.dueDate,
      isCompleted: updated.isCompleted,
      categoryId: updated.categoryId
    })
    .subscribe(() => {
      this.closeEdit();
      this.search();
    });
  }
}
