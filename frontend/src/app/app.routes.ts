import {Routes} from '@angular/router';
import {RegisterComponent} from './pages/register/register.component';
import {MainComponent} from './pages/main/main.component';
import {LoginComponent} from './pages/login/login.component';
import {canActivateAuth} from './guards/auth.gruard';
import {MainLayoutComponent} from './layout/main-layout/main-layout.component';
import {AuthLayoutComponent} from './layout/auth-layout/auth-layout.component';
import {TasksPageComponent} from './pages/tasks-page/tasks-page.component';

export const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, children: [
      {
        path: '',
        component: MainComponent,
      },
      {
        path: 'categories/:categoryId',
        component: TasksPageComponent,
      }
    ],
    canActivate: [canActivateAuth]
  },
  {
    path: '', component: AuthLayoutComponent, children: [
      {path: 'login', component: LoginComponent},
      {path: 'register', component: RegisterComponent},
    ]
  }
];
