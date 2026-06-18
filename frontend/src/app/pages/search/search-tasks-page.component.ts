import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterLink} from '@angular/router';

import { TaskService } from '../../services/task.service';

import { PaginationComponent } from '../../common-ui/pagination/pagination.component';
import {TaskListComponent} from '../../common-ui/task-list/task-list.component';
import {EditTaskModalComponent} from '../../common-ui/edit-task-modal/edit-task-modal.component';
import {SearchFormComponent} from './search-form/search-form.component';
import {Task} from '../../interfaces/models/task';
import {PagedData} from '../../interfaces/models/paged-data';

@Component({
  selector: 'app-search-page',
  imports: [
    TaskListComponent,
    PaginationComponent,
    EditTaskModalComponent,
    SearchFormComponent,
    RouterLink
  ],
  templateUrl: './search-tasks-page.component.html'
})
export class SearchPageComponent implements OnInit {

  private taskService = inject(TaskService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  pagedTasks: PagedData<Task> | null = null;

  currentPage = 1;
  pageSize = 6;

  query = '';
  categoryId: number | null = null;

  isLoading = false;
  isError = false;

  selectedTask: Task | null = null;
  isEditOpen = false;

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      this.query = params.get('query') ?? '';
      this.currentPage = Number(params.get('page') ?? 1);

      const categoryParam = params.get('categoryId');

      this.categoryId =
        !categoryParam || categoryParam === 'any'
          ? null
          : Number(categoryParam);

      this.search();
    });
  }

  search() {
    this.isLoading = true;
    this.isError = false;

    this.taskService
    .searchTasks(this.query, this.categoryId, this.currentPage, this.pageSize)
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

  onSearch(search: {query: string, categoryId: number | null}) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        query: search.query,
        categoryId: search.categoryId ?? 'any',
        page: 1
      }
    });
  }

  onDeleteTask(taskId: number) {
    this.taskService.deleteTask(taskId)
    .subscribe(() => {
      this.search();
    });
  }

  onPageChange(page: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {
        query: this.query,
        categoryId: this.categoryId ?? 'any',
        page
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

  onEdit(task: Task) {
    this.selectedTask = task;
    this.isEditOpen = true;
  }

  closeEdit() {
    this.selectedTask = null;
    this.isEditOpen = false;
  }

  onSaveEditedTask(edited: Task) {
    this.taskService.updateTask(edited.id, {
      title: edited.title,
      dueDate: edited.dueDate,
      isCompleted: edited.isCompleted,
      categoryId: edited.categoryId
    })
    .subscribe(() => {
      this.closeEdit();
      this.search();
    });
  }
}
