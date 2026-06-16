import {Component, inject, Input} from '@angular/core';
import {Category} from '../../interfaces/category';
import {Router, RouterLink} from '@angular/router';

@Component({
  selector: 'app-category-card',
  imports: [
    RouterLink
  ],
  templateUrl: './category-card.component.html'
})
export class CategoryCardComponent {
  @Input() category!: Category;

  private router = inject(Router);
}
