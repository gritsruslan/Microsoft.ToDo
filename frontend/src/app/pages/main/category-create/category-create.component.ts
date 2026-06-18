import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ReactiveFormsModule, FormControl, FormGroup, Validators } from '@angular/forms';
import {VALIDATION} from '../../../contants/validation.constants';

@Component({
  selector: 'app-category-create',
  imports: [ReactiveFormsModule],
  templateUrl: './category-create.component.html',
})
export class CategoryCreateComponent {

  @Input() error: string | null = null;
  @Input() isLoading = false;

  @Output() create = new EventEmitter<string>();

  form = new FormGroup({
    name: new FormControl<string>('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.maxLength(VALIDATION.CATEGORY_NAME_MAX_LENGTH)
      ]
    })
  });

  get nameControl() {
    return this.form.controls.name;
  }

  onSubmit() {
    if (this.form.invalid) return;

    const name = this.nameControl.value.trim();
    this.create.emit(name);

    this.form.reset();
  }

  protected readonly VALIDATION = VALIDATION;
}
