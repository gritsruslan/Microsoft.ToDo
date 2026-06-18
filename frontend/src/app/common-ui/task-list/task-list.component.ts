import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink } from '@angular/router';
import {TaskCardComponent} from '../task-card/task-card.component';
import {PagedData} from '../../interfaces/models/paged-data';
import {Task} from '../../interfaces/models/task';

@Component({
  selector: 'app-task-list',
  imports: [TaskCardComponent],
  templateUrl: './task-list.component.html'
})
export class TaskListComponent {

  @Input({required: true}) pagedTasks!: PagedData<Task>;
  @Input() isLoading = false;
  @Input() isError = false;
  @Input() showCategoryName: boolean = false;

  @Output() deletedTask = new EventEmitter<number>();
  @Output() editTask = new EventEmitter<Task>();
  @Output() toggleIsCompleted = new EventEmitter<Task>();

  onTaskDeleted(id: number) {
    this.deletedTask.emit(id);
  }

  onTaskEdit(task: Task) {
    this.editTask.emit(task);
  }

  onTaskToggle(task: Task) {
    this.toggleIsCompleted.emit(task);
  }
}
