import { Component, EventEmitter, Input, Output } from '@angular/core';
import { DatePipe } from '@angular/common';
import {Task} from '../../interfaces/models/task';

@Component({
  selector: 'app-task-card',
  imports: [DatePipe],
  templateUrl: './task-card.component.html'
})
export class TaskCardComponent {
  @Input({ required: true }) task!: Task;
  @Input() showCategoryName = false;

  @Output() deleted = new EventEmitter<number>();
  @Output() edit = new EventEmitter<Task>();
  @Output() toggle = new EventEmitter<Task>();

  toggleCompleted() {
    this.toggle.emit(this.task);
  }

  deleteTask() {
    this.deleted.emit(this.task.id);
  }
}
