import {Routes} from '@angular/router';
import {canActivateAuth} from './guards/auth.gruard';
import {MainLayoutComponent} from './layout/main-layout/main-layout.component';
import {AuthLayoutComponent} from './layout/auth-layout/auth-layout.component';
import {TasksPageComponent} from './pages/tasks-page/tasks-page.component';
import { SearchPageComponent } from './pages/search/search-tasks-page.component';
import {MainPageComponent} from './pages/main/main-page.component';
import {NotFoundPageComponent} from './pages/not-found/not-found-page.component';
import {RegisterPageComponent} from './pages/register/register-page.component';
import {LoginPageComponent} from './pages/login/login-page.component';

export const routes: Routes = [
  {
    path: '', component: MainLayoutComponent, children: [
      {
        path: '',
        component: MainPageComponent,
      },
      {
        path: 'categories/:categoryId',
        component: TasksPageComponent,
      },
      {
        path: 'search',
        component: SearchPageComponent,
      }
    ],
    canActivate: [canActivateAuth]
  },
  {
    path: '',
    component: AuthLayoutComponent,
    children: [
      {
        path: 'login',
        component: LoginPageComponent
      },
      {
        path: 'register',
        component: RegisterPageComponent
      },
    ]
  },
  {
    path: '**',
    component: NotFoundPageComponent
  }
];
