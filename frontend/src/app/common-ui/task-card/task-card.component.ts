import { Component, Input } from '@angular/core';
import { Task } from '../../interfaces/task';
import {DatePipe} from '@angular/common';

@Component({
  selector: 'app-task-card',
  templateUrl: './task-card.component.html',
  imports: [
    DatePipe
  ]
})
export class TaskCardComponent {
  @Input({ required: true }) task!: Task;
  @Input() showCategoryName = false;
}
