import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink } from '@angular/router';
import {PagedData} from '../../../interfaces/paged-data';
import {TaskCardComponent} from '../../../common-ui/task-card/task-card.component';
import {Task} from '../../../interfaces/task';

@Component({
  selector: 'app-task-list',
  imports: [TaskCardComponent, RouterLink],
  templateUrl: './task-list.component.html'
})
export class TaskListComponent {

  @Input() pagedTasks: PagedData<Task> | null = null;
  @Input() isLoading = false;
  @Input() isError = false;
  @Input() categoryName: string | null = null;

  @Output() deleted = new EventEmitter<number>();
  @Output() edit = new EventEmitter<Task>();
  @Output() toggle = new EventEmitter<Task>();

  onTaskDeleted(id: number) {
    this.deleted.emit(id);
  }

  onTaskEdit(task: Task) {
    this.edit.emit(task);
  }

  onTaskToggle(task: Task) {
    this.toggle.emit(task);
  }
}
