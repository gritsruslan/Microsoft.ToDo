import {Component, inject, OnInit} from '@angular/core';
import {ActivatedRoute} from '@angular/router';

import {NavbarComponent} from '../../common-ui/navbar/navbar.component';

import {TaskCreateComponent} from './task-create/task-create.component';
import {TaskListComponent} from './task-list/task-list.component';

import {TaskService} from '../../services/task.service';

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

  categoryId!: number;

  isLoadingTasks = true;
  isErrorLoadingTasks = false;

  ngOnInit() {

    this.categoryId = Number(
      this.route.snapshot.paramMap.get('categoryId')
    );

    this.taskService
    .searchTasks(null, this.categoryId, 1, 20
    )
    .subscribe({
      next: () => {
        this.isLoadingTasks = false;
        this.isErrorLoadingTasks = false;
      },
      error: () => {
        this.isLoadingTasks = false;
        this.isErrorLoadingTasks = true;
      }
    });
  }
}
