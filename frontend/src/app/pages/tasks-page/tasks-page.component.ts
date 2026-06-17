import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';

import {TaskService} from '../../services/task.service';
import {TaskCreateComponent} from './task-create/task-create.component';
import {TaskListComponent} from './task-list/task-list.component';
import {CategoryService} from '../../services/category.service';
import {PaginationComponent} from '../../common-ui/pagination/pagination.component';

@Component({
  selector: 'app-tasks-page',
  imports: [
    TaskCreateComponent,
    TaskListComponent,
    PaginationComponent
  ],
  templateUrl: './tasks-page.component.html'
})
export class TasksPageComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  taskService = inject(TaskService);
  categoryService = inject(CategoryService);
  currentPage: number = 1;
  pageSize: number = 7;

  categoryId!: number;
  categoryName: string | null = null;

  isLoadingTasks = true;
  isErrorLoadingTasks = false;

  ngOnInit() {
    this.categoryId = Number(
      this.route.snapshot.paramMap.get('categoryId')
    );

    this.route.queryParamMap.subscribe(params => {
      this.currentPage = Number(params.get('page') ?? 1);
      this.loadCategory();
      this.loadTasks(this.currentPage);
    });
  }

  loadTasks(page: number = 1) {
    this.currentPage = page;
    this.isLoadingTasks = true;

    this.taskService.searchTasks(null, this.categoryId, page, this.pageSize)
    .subscribe({
      next: () => {
        this.isLoadingTasks = false;
      },
      error: () => {
        this.isLoadingTasks = false;
        this.isErrorLoadingTasks = true;
      }
    });
  }

  onPageChange(page: number) {
    this.router.navigate([], {
      relativeTo: this.route,
      queryParams: {page},
      queryParamsHandling: 'merge'
    });
  }

  loadCategory() {
    this.categoryService.getCategoryById(this.categoryId)
    .subscribe(c => this.categoryName = c.name);
  }
}
