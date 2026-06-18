import {Component, Input} from '@angular/core';
import {CategoryCardComponent} from '../../../common-ui/category-card/category-card.component';
import {Category} from '../../../interfaces/models/category';
import {RouterLink} from '@angular/router';

@Component({
  selector: 'app-category-list',
  imports: [
    CategoryCardComponent,
    RouterLink
  ],
  templateUrl: './category-list.component.html',
})
export class CategoryListComponent {
  @Input({required: true}) categories!: Category[];
  @Input() isError: boolean = false;
}
