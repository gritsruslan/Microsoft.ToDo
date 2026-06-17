import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-category-create',
  imports: [FormsModule],
  templateUrl: './category-create.component.html',
})
export class CategoryCreateComponent {

  categoryName = '';

  @Input()
  error: string | null = null;

  @Input()
  isLoading = false;

  @Output()
  create = new EventEmitter<string>();

  get isValidCategoryName() {
    return this.categoryName.trim().length >= 1
      && this.categoryName.trim().length <= 40;
  }

  createCategory() {
    const name = this.categoryName.trim();
    if (!name) {
      return;
    }
    this.create.emit(name);
  }

  clear() {
    this.categoryName = '';
  }
}
