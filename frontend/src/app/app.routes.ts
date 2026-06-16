import { Routes } from '@angular/router';
import {RegisterComponent} from './pages/register/register.component';
import {MainComponent} from './pages/main/main.component';
import {LoginComponent} from './pages/login/login.component';
import {canActivateAuth} from './guards/auth.gruard';

export const routes: Routes = [
  {path: '', component: MainComponent, canActivate: [canActivateAuth]},
  {path: 'register', component: RegisterComponent},
  {path: 'login', component: LoginComponent}
];
