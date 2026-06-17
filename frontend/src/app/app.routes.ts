import {Routes} from '@angular/router';
import {RegisterComponent} from './pages/register/register.component';
import {LoginComponent} from './pages/login/login.component';
import {canActivateAuth} from './guards/auth.gruard';
import {MainLayoutComponent} from './layout/main-layout/main-layout.component';
import {AuthLayoutComponent} from './layout/auth-layout/auth-layout.component';
import {TasksPageComponent} from './pages/tasks-page/tasks-page.component';
import { SearchPageComponent } from './pages/search/search-tasks-page.component';
import {MainPageComponent} from './pages/main/main-page.component';
import {NotFoundPageComponent} from './pages/not-found/not-found-page.component';

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
        component: LoginComponent
      },
      {
        path: 'register',
        component: RegisterComponent
      },
    ]
  },
  {
    path: '**',
    component: NotFoundPageComponent
  }
];
