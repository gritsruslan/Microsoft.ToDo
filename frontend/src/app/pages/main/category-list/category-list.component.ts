import {Component, Input} from '@angular/core';
import {CategoryCardComponent} from '../../../common-ui/category-card/category-card.component';
import {Category} from '../../../interfaces/category';

@Component({
  selector: 'app-category-list',
  imports: [
    CategoryCardComponent
  ],
  templateUrl: './category-list.component.html',
})
export class CategoryListComponent {
  @Input() categories: Category[] = [];
  @Input() isError: boolean = false;
  @Input() isLoading: boolean = false;
}
