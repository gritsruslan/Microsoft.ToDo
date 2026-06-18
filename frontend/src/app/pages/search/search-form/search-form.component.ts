import { Component, EventEmitter, inject, Output } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import { AsyncPipe } from '@angular/common';
import { CategoryService } from '../../../services/category.service';
import { Category } from '../../../interfaces/models/category';
import {VALIDATION} from '../../../contants/validation.constants';

@Component({
  selector: 'app-search-form',
  imports: [ReactiveFormsModule, AsyncPipe],
  templateUrl: './search-form.component.html'
})
export class SearchFormComponent {

  private categoryService = inject(CategoryService);

  categories$ = this.categoryService.getCategories();

  @Output() search = new EventEmitter<{
    query: string;
    categoryId: number | null;
  }>();

  form = new FormGroup({
    query: new FormControl<string>('', {
      nonNullable: true,
      validators: [
        Validators.maxLength(VALIDATION.TASK_TITLE_MAX_LENGTH)
      ]
    }),
    categoryId: new FormControl<number | null>(null)
  });

  get queryControl() {
    return this.form.controls.query;
  }

  onSubmit() {
    if (this.form.invalid) return;

    this.search.emit({
      query: this.queryControl.value.trim(),
      categoryId: this.form.controls.categoryId.value
    });
  }

  protected readonly VALIDATION = VALIDATION;
}
