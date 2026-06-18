import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-search-form',
  imports: [FormsModule],
  templateUrl: './search-form.component.html'
})
export class SearchFormComponent {
  @Output() search = new EventEmitter<string>();

  query = '';

  onSearch() {
    this.search.emit(this.query.trim());
  }
}
