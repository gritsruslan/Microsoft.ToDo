import {Component, inject, OnInit} from '@angular/core';
import { finalize } from 'rxjs';

import { CategoryService } from '../../services/category.service';
import {CategoryCreateComponent} from './category-create/category-create.component';
import {CategoryListComponent} from './category-list/category-list.component';
import {Category} from '../../interfaces/models/category';

@Component({
  selector: 'app-main',
  imports: [
    CategoryCreateComponent,
    CategoryListComponent
  ],
  templateUrl: './main-page.component.html'
})
export class MainPageComponent implements OnInit{

  private categoryService = inject(CategoryService);

  categories: Category[] | null = null;
  isLoadingCategories = true;
  isErrorLoadingCategories = false;

  isCreatingCategory = false;
  createCategoryError: string | null = null;

  ngOnInit() {
    this.loadCategories();
  }

  loadCategories() {
    this.isLoadingCategories = true;

    this.categoryService.getCategories()
    .subscribe({
      next: categories => {
        this.categories = categories;
        this.isLoadingCategories = false;
        this.isErrorLoadingCategories = false;
      },
      error: () => {
        this.isLoadingCategories = false;
        this.isErrorLoadingCategories = true;
      }
    });
  }

  onCreateCategory(name: string) {

    this.isCreatingCategory = true;
    this.createCategoryError = null;

    this.categoryService.createCategory(name)
    .pipe(
      finalize(() => this.isCreatingCategory = false)
    )
    .subscribe({
      next: () => {
        this.loadCategories();
      },
      error: () => {
        this.createCategoryError = 'Failed to create category';
      }
    });
  }
}
