import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';
import { RouteNames } from '../../../shared/consts/routes';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,
            ReactiveFormsModule,
            RouterLink],
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  form: FormGroup;
  isLoading = false;
  showPassword = false;

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {
    this.form = this.fb.group({
      username: ['', [Validators.required, Validators.minLength(3)]],
      password: ['', [Validators.required, Validators.minLength(6)]]
    });
  }

  get username() { return this.form.get('username')!; }
  get password() { return this.form.get('password')!; }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.authService.login(this.form.value).subscribe({
      next: (res) => {
        this.toastService.success(`Dobrodošli, ${res.fullName}!`);
        if (res.role === 'Manager') {
          this.router.navigate([`/${RouteNames.ManagerDashboard}`]);
        } else {
          this.router.navigate([`/${RouteNames.MechanicDashboard}`]);
        }
      },
      error: (err) => {
        const message = err?.error?.error ?? 'Prijava nije uspela. Pokušajte ponovo.';
        this.toastService.error(message);
        this.isLoading = false;
      }
    });
  }
}