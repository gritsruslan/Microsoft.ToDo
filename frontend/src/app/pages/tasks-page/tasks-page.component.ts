import { Component, inject, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { TaskService } from '../../services/task.service';
import {TaskCreateComponent} from './task-create/task-create.component';
import {TaskListComponent} from './task-list/task-list.component';
import {CategoryService} from '../../services/category.service';

@Component({
  selector: 'app-tasks-page',
  imports: [
    TaskCreateComponent,
    TaskListComponent
  ],
  templateUrl: './tasks-page.component.html'
})
export class TasksPageComponent implements OnInit {

  private route = inject(ActivatedRoute);
  taskService = inject(TaskService);
  categoryService = inject(CategoryService);

  categoryId!: number;
  categoryName: string | null = null;

  isLoadingTasks = true;
  isErrorLoadingTasks = false;

  ngOnInit() {
    this.categoryId = Number(
      this.route.snapshot.paramMap.get('categoryId')
    );

    this.loadCategory();
    this.loadTasks();
  }

  loadTasks() {

    this.isLoadingTasks = true;
    this.isErrorLoadingTasks = false;

    this.taskService
    .searchTasks(null, this.categoryId, 1, 20)
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

  loadCategory() {
    this.categoryService.getCategoryById(this.categoryId)
    .subscribe(c => this.categoryName = c.name);
  }
}
