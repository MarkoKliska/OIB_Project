import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../shared/services/auth.service';
import { ToastService } from '../../../shared/services/toast.service';
import { RouteNames } from '../../../shared/consts/routes';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent {
  form: FormGroup;
  isLoading = false;
  showPassword = false;

  roles = [
    { value: 'Manager', label: 'Manager' },
    { value: 'Mechanic', label: 'Mechanic' }
  ];

  constructor(
    private fb: FormBuilder,
    private authService: AuthService,
    private toastService: ToastService,
    private router: Router
  ) {
    this.form = this.fb.group({
      firstName: ['', [Validators.required, Validators.minLength(2)]],
      lastName:  ['', [Validators.required, Validators.minLength(2)]],
      username:  ['', [Validators.required, Validators.minLength(3)]],
      password:  ['', [Validators.required, Validators.minLength(6)]],
      role:      ['Mechanic', Validators.required]
    });
  }

  get firstName() { return this.form.get('firstName')!; }
  get lastName()  { return this.form.get('lastName')!;  }
  get username()  { return this.form.get('username')!;  }
  get password()  { return this.form.get('password')!;  }
  get role()      { return this.form.get('role')!;      }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  onSubmit(): void {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    this.isLoading = true;
    this.authService.register(this.form.value).subscribe({
      next: (res) => {
        this.toastService.success(`Account created! Welcome, ${res.firstName}!`);
        if (res.role === 'Manager') {
          this.router.navigate([`/${RouteNames.ManagerDashboard}`]);
        } else {
          this.router.navigate([`/${RouteNames.MechanicDashboard}`]);
        }
      },
      error: (err) => {
        const message = err?.error?.error ?? 'Registration failed. Please try again.';
        this.toastService.error(message);
        this.isLoading = false;
      }
    });
  }
}