import { Routes } from '@angular/router';
import {RegisterComponent} from './pages/register/register.component';
import {MainComponent} from './pages/main/main.component';

export const routes: Routes = [
  {path: '', component: MainComponent},
  {path: 'register', component: RegisterComponent}
];
