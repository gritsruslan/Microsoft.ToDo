import { Component, inject } from '@angular/core';
import { CategoryService } from '../../../services/category.service';
import { FormsModule } from '@angular/forms';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-category-create',
  imports: [FormsModule],
  templateUrl: './category-create.component.html',
})
export class CategoryCreateComponent {
  private categoryService = inject(CategoryService);

  categoryName: string = '';
  error: string | null = null;
  isLoading = false;

  get isValidCategoryName() {
    return this.categoryName.trim().length > 1 && this.categoryName.trim().length < 40;
  }

  createCategory() {
    const name = this.categoryName.trim();

    if (!name) return;

    this.isLoading = true;
    this.error = null;

    this.categoryService.createCategory(name)
    .pipe(finalize(() => this.isLoading = false))
    .subscribe({
      next: () => this.categoryName = '',
      error: () => this.error = 'Failed to create category'
    });
  }
}
