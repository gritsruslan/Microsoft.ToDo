import { Component, EventEmitter, inject, Input, Output } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { TaskService } from '../../../services/task.service';

@Component({
  selector: 'app-task-create',
  imports: [ReactiveFormsModule],
  templateUrl: './task-create.component.html'
})
export class TaskCreateComponent {

  @Input({ required: true })
  categoryId!: number;

  @Output()
  created = new EventEmitter<void>();

  private taskService = inject(TaskService);

  isCreating = false;
  error: string | null = null;

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

    this.isCreating = true;
    this.error = null;

    const value = this.form.getRawValue();

    this.taskService.createTask({
      title: value.title.trim(),
      categoryId: this.categoryId,
      dueDate: value.dueDate
    })
    .subscribe({
      next: () => {

        this.form.reset({
          title: '',
          dueDate: null
        });

        this.isCreating = false;

        this.created.emit();
      },
      error: () => {
        this.error = 'Failed to create task';
        this.isCreating = false;
      }
    });
  }
}
