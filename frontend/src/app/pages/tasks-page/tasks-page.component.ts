import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { TaskService } from '../../services/task.service';
import { CategoryService } from '../../services/category.service';

import { Task } from '../../interfaces/task';
import { PagedData } from '../../interfaces/paged-data';

import { TaskCreateComponent } from './task-create/task-create.component';
import { PaginationComponent } from '../../common-ui/pagination/pagination.component';
import { EditTaskModalComponent } from '../../common-ui/edit-task-modal/edit-task-modal.component';
import {CreateTaskRequest} from '../../interfaces/create-task-request';
import {TaskListComponent} from '../../common-ui/task-list/task-list.component';

@Component({
  selector: 'app-tasks-page',
  imports: [
    TaskCreateComponent,
    TaskListComponent,
    PaginationComponent,
    EditTaskModalComponent
  ],
  templateUrl: './tasks-page.component.html'
})
export class TasksPageComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private router = inject(Router);

  private taskService = inject(TaskService);
  private categoryService = inject(CategoryService);

  currentPage = 1;
  pageSize = 7;

  categoryId!: number;
  categoryName: string | null = null;

  pagedTasks: PagedData<Task> | null = null;
  isLoadingTasks = false;
  isErrorLoadingTasks = false;

  selectedTask: Task | null = null;
  isEditOpen = false;

  createError: string | null = null;
  isCreatingTask = false;

  ngOnInit() {
    this.categoryId = Number(
      this.route.snapshot.paramMap.get('categoryId')
    );

    this.route.queryParamMap.subscribe(params => {
      this.currentPage = Number(params.get('page') ?? 1);
      this.loadCategory();
      this.loadTasks();
    });
  }

  loadTasks() {
    this.isLoadingTasks = true;
    this.isErrorLoadingTasks = false;

    this.taskService
    .searchTasks(null, this.categoryId, this.currentPage, this.pageSize)
    .subscribe({
      next: (data) => {
        this.pagedTasks = data;
        this.isLoadingTasks = false;
      },
      error: () => {
        this.isLoadingTasks = false;
        this.isErrorLoadingTasks = true;
      }
    });
  }

  onCreateTask(request: CreateTaskRequest) {
    this.createError = null;
    this.isCreatingTask = true;

    this.taskService.createTask(request)
    .subscribe({
      next: () => {
        this.isCreatingTask = false;
        this.loadTasks();
      },
      error: () => {
        this.isCreatingTask = false;
        this.createError = 'Failed to create task';
      }
    });
  }

  loadCategory() {
    this.categoryService
    .getCategoryById(this.categoryId)
    .subscribe(c => {
      this.categoryName = c.name;
    });
  }

  onDelete(taskId: number) {
    this.taskService.deleteTask(taskId)
    .subscribe(() => {
      this.loadTasks();
    });
  }

  onEdit(task: Task) {
    this.selectedTask = task;
    this.isEditOpen = true;
  }

  closeEdit() {
    this.isEditOpen = false;
    this.selectedTask = null;
  }

  onSaveTask(updated: Task) {
    this.taskService.updateTask(updated.id, updated)
    .subscribe(() => {
      this.closeEdit();
      this.loadTasks();
    });
  }

  onToggle(task: Task) {
    this.taskService.updateTask(task.id, {
      title: task.title,
      dueDate: task.dueDate,
      isCompleted: task.isCompleted,
      categoryId: task.categoryId
    })
    .subscribe(() => {
      this.loadTasks();
    });
  }

  onPageChange(page: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: { page },
      queryParamsHandling: 'merge'
    });
  }
}
