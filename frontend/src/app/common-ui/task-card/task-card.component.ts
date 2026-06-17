import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { Task } from '../../interfaces/task';
import { TaskService } from '../../services/task.service';
import { UpdateTaskRequest } from '../../interfaces/update-task-request';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-task-card',
  imports: [DatePipe],
  templateUrl: './task-card.component.html'
})
export class TaskCardComponent {

  private taskService = inject(TaskService);

  @Input({ required: true })
  task!: Task;

  @Input()
  showCategoryName = false;

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
