import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({ providedIn: 'root' })
export class NoAuthGuard implements CanActivate {
  constructor(private authService: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (!this.authService.isLoggedIn()) {
      return true;
    }

    // Redirect to role-appropriate dashboard
    const role = this.authService.getUserRole();
    if (role === 'Manager') {
      this.router.navigate(['/manager']);
    } else {
      this.router.navigate(['/mechanic']);
    }
    return false;
  }
}