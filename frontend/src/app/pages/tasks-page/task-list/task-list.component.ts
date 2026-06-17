import { Component, Input } from '@angular/core';

import { Task } from '../../../interfaces/task';
import { PagedData } from '../../../interfaces/paged-data';

import { TaskCardComponent } from '../../../common-ui/task-card/task-card.component';

@Component({
  selector: 'app-task-list',
  imports: [
    TaskCardComponent
  ],
  templateUrl: './task-list.component.html'
})
export class TaskListComponent {
  @Input() pagedTasks: PagedData<Task> | null = null;
  @Input() isLoading = false;
  @Input() isError = false;
}
