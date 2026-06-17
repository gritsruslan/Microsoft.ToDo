import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-form',
  imports: [FormsModule],
  templateUrl: './search-form.component.html'
})
export class SearchFormComponent {

  query = '';

  @Output()
  search = new EventEmitter<string>();

  onSearch() {
    this.search.emit(this.query.trim());
  }
}
