import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../../services/task.service';
import {CreateTaskRequest} from '../../../interfaces/create-task-request';

@Component({
  selector: 'app-task-create',
  imports: [ReactiveFormsModule],
  templateUrl: './task-create.component.html'
})
export class TaskCreateComponent {

  @Input({ required: true })
  categoryId!: number;

  @Input()
  error: string | null = null;

  @Input()
  isCreating = false;

  @Output()
  create = new EventEmitter<CreateTaskRequest>();


  form = new FormGroup({
    title: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required]
    }),
    dueDate: new FormControl<string | null>(null)
  });

  createTask() {

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    this.create.emit({
      title: value.title.trim(),
      categoryId: this.categoryId,
      dueDate: value.dueDate
    });
  }
}
