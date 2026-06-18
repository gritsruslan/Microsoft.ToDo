import {Component, EventEmitter, Input, Output, OnChanges, inject} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CategoryService } from '../../services/category.service';
import {AsyncPipe} from '@angular/common';
import {Task} from '../../interfaces/models/task';

@Component({
  selector: 'app-edit-task-modal',
  imports: [FormsModule, AsyncPipe],
  templateUrl: './edit-task-modal.component.html'
})
export class EditTaskModalComponent implements OnChanges {

  @Input() isOpen = false;
  @Input({required: true}) task!: Task;

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<Task>();

  private categoryService = inject(CategoryService);

  categories$ = this.categoryService.getCategories();

  form!: {
    title: string;
    dueDate: string | null;
    categoryId: number;
    isCompleted: boolean;
  };

  ngOnChanges() {
    if (!this.task) return;

    this.form = {
      title: this.task.title,
      dueDate: this.task.dueDate
        ? new Date(this.task.dueDate).toISOString().slice(0, 16)
        : null,
      categoryId: this.task.categoryId,
      isCompleted: this.task.isCompleted
    };
  }

  onClose() {
    this.close.emit();
  }

  onSave() {
    if (!this.task) return;

    this.save.emit({
      ...this.task,
      title: this.form.title,
      dueDate: this.form.dueDate,
      categoryId: this.form.categoryId,
      isCompleted: this.form.isCompleted
    });
  }
}
