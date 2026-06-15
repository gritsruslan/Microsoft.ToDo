import {Component, inject} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {FormControl, FormGroup, ReactiveFormsModule, Validators} from '@angular/forms';
import {Router} from '@angular/router';
import {catchError, throwError} from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  authService = inject(AuthService);
  router = inject(Router);
  validationErrors: string[] = [];

  form = new FormGroup<{
    email: FormControl<string>,
    password: FormControl<string>
  }>({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email]
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.minLength(8),
        Validators.maxLength(100),
        Validators.pattern(/^(?=.*[a-z])(?=.*\d).+$/)
      ]
    })
  })

  onSubmit() {
    if(!this.form.valid) return;

    this.authService.register(this.form.getRawValue()).pipe(
      catchError(err => {
        console.error(err)
        this.validationErrors = err.error.errors ?? [err.error.message]
        return throwError(() => err)
      })
    ).subscribe(() => {
      this.router.navigate(['/'])
    });
  }
}

