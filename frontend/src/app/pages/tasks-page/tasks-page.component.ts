import { Component, inject, OnInit } from '@angular/core';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';

import { TaskService } from '../../services/task.service';
import { CategoryService } from '../../services/category.service';

import { TaskCreateComponent } from './task-create/task-create.component';
import { PaginationComponent } from '../../common-ui/pagination/pagination.component';
import { EditTaskModalComponent } from '../../common-ui/edit-task-modal/edit-task-modal.component';
import {TaskListComponent} from '../../common-ui/task-list/task-list.component';
import {CreateTaskRequest} from '../../interfaces/requests/create-task-request';
import {PagedData} from '../../interfaces/models/paged-data';
import {Task} from '../../interfaces/models/task';

@Component({
  selector: 'app-tasks-page',
  imports: [
    TaskCreateComponent,
    TaskListComponent,
    PaginationComponent,
    EditTaskModalComponent,
    RouterLink
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
  isErrorEditingTask: boolean = false;

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

  onDeleteTask(taskId: number) {
    this.taskService.deleteTask(taskId)
    .subscribe(() => {
      this.loadTasks();
    });
  }

  onStartEdit(task: Task) {
    this.selectedTask = task;
    this.isEditOpen = true;
  }

  onCloseEdit() {
    this.closeEdit();
  }

  closeEdit() {
    this.isErrorEditingTask = false;
    this.isEditOpen = false;
    this.selectedTask = null;
  }

  onSaveEditedTask(edited: Task) {
    this.taskService.updateTask(edited.id, edited)
    .subscribe({
      next: () => {
        this.isErrorEditingTask = false;
        this.closeEdit();
        this.loadTasks();
      },
      error: () => {
        this.isErrorEditingTask = true;
      }
    });
  }

  onToggleIsCompleted(task: Task) {
    const updated = !task.isCompleted;

    this.taskService.updateTask(task.id, {
      ...task,
      isCompleted: updated
    }).subscribe(() => {

      const list = this.pagedTasks?.items;
      if (!list) return;

      const t = list.find(x => x.id === task.id);
      if (t) t.isCompleted = updated;
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
