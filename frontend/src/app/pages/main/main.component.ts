import {Component, inject} from '@angular/core';
import { CategoryService } from '../../services/category.service';
import {CategoryListComponent} from './category-list/category-list.component';
import {CategoryCreateComponent} from './category-create/category-create.component';
import {finalize} from 'rxjs';

@Component({
  selector: 'app-main',
  imports: [
    CategoryListComponent,
    CategoryCreateComponent
  ],
  templateUrl: './main.component.html'
})
export class MainComponent {
  categoryService = inject(CategoryService)

  isLoadingCategories = true;
  isErrorLoadingCategories = false;

  ngOnInit() {
    this.categoryService.getCategories().pipe(
      finalize(() => this.isLoadingCategories = false)
    ).subscribe({
      next: () => this.isErrorLoadingCategories = false,
      error: () => this.isErrorLoadingCategories = true
    });
  }
}
