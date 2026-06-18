import {Component, Input} from '@angular/core';
import {RouterLink} from '@angular/router';
import {Category} from '../../interfaces/models/category';

@Component({
  selector: 'app-category-card',
  imports: [
    RouterLink
  ],
  templateUrl: './category-card.component.html'
})
export class CategoryCardComponent {
  @Input({required: true}) category!: Category;
}
