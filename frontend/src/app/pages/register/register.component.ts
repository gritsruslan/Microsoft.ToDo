import {Component, inject} from '@angular/core';
import {AuthService} from '../../services/auth.service';
import {
  AbstractControl,
  FormControl,
  FormGroup,
  ReactiveFormsModule,
  ValidationErrors,
  Validators
} from '@angular/forms';
import {Router, RouterLink} from '@angular/router';
import {catchError, throwError} from 'rxjs';

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    RouterLink
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  private authService = inject(AuthService);
  private router = inject(Router);

  apiErrors: string[] = [];
  isRegistering = false;

  form = new FormGroup<{
    email: FormControl<string>,
    password: FormControl<string>,
    confirmPassword: FormControl<string>
  }>({
    email: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required, Validators.email]
    }),
    password: new FormControl('', {
      nonNullable: true,
      validators: [
        Validators.required,
        Validators.minLength(6),
        Validators.maxLength(30),
        Validators.pattern(/^(?=.*[a-z])(?=.*\d).+$/)
      ]
    }),
    confirmPassword: new FormControl('', {
      nonNullable: true,
      validators: [Validators.required]
    })
  }, { validators: [RegisterComponent.validatePasswordsMatch] })

  onSubmit() {
    if(!this.form.valid) return;

    this.isRegistering = true;

    this.authService.register(this.form.getRawValue()).pipe(
      catchError(err => {
        console.error(err)
        this.apiErrors = [err.error.title ?? "Failed to register"];
        return throwError(() => err)
      })
    ).subscribe(() => {
      this.isRegistering = false;
      this.router.navigate(['/'])
    });
  }

  private static validatePasswordsMatch(control: AbstractControl) : ValidationErrors | null {
    const password = control.get('password')?.value;
    const confirmPassword = control.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }
}

