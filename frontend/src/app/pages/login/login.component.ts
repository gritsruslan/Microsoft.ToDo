import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  Validators
} from '@angular/forms';
import { catchError, throwError } from 'rxjs';

import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  apiErrors: string[] = [];
  isLoggingIn = false;

  form = new FormGroup({
    email: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.email
      ]
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required
      ]
    })
  });

  onSubmit() {
    this.apiErrors = [];

    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoggingIn = true;

    this.authService.login(this.form.getRawValue())
    .pipe(
      catchError(err => {
        this.apiErrors = [
          err.error?.title ?? 'Failed to login'
        ];

        this.isLoggingIn = false;

        return throwError(() => err);
      })
    )
    .subscribe(() => {
      this.isLoggingIn = false;
      this.router.navigate(['/']);
    });
  }
}
