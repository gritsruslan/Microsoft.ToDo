import {Component, EventEmitter, inject, Input, Output} from '@angular/core';
import { Task } from '../../interfaces/task';
import { TaskService } from '../../services/task.service';
import {UpdateTaskRequest} from '../../interfaces/update-task-request';
import {DatePipe, NgClass} from '@angular/common';

@Component({
  selector: 'app-task-card',
  imports: [
    DatePipe,
    NgClass
  ],
  templateUrl: './task-card.component.html'
})
export class TaskCardComponent {

  private taskService = inject(TaskService);

  @Input({ required: true })
  task!: Task;

  @Input()
  showCategoryName = false;

  @Output() deleted = new EventEmitter<void>();

  toggleCompleted() {
    const updated : UpdateTaskRequest = {
      title: this.task.title,
      dueDate: this.task.dueDate,
      isCompleted: !this.task.isCompleted
    };

    this.taskService.updateTask(this.task.id, updated)
    .subscribe(() => {
      this.task.isCompleted = !this.task.isCompleted;
    });
  }

  deleteTask() {
    this.taskService.deleteTask(this.task.id).subscribe(
      () => this.deleted.emit());
  }
}
