import {CanActivateFn, Router} from '@angular/router';
import {AuthService} from '../services/auth.service';
import {inject} from '@angular/core';
import {catchError, map, of} from 'rxjs';

export const canActivateAuth: CanActivateFn = () => {
  const authService = inject(AuthService);
  const router = inject(Router);

  if(authService.isAuth) {
    return true;
  }

  return authService.getMe().pipe(
    map(() => true),
    catchError(() => {
      return of(router.createUrlTree(['/login']));
    })
  );
};
