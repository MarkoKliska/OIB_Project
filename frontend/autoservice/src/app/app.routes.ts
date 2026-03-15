import { Routes } from '@angular/router';
import { RouteNames } from './shared/consts/routes';
import { AuthGuard } from './shared/guards/auth.guard';
import { NoAuthGuard } from './shared/guards/no-auth.guard';
import { RoleGuard } from './shared/guards/role.guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: RouteNames.Login,
    pathMatch: 'full'
  },
  {
    path: RouteNames.Login,
    loadComponent: () =>
      import('./modules/auth/login/login.component').then(m => m.LoginComponent),
    canActivate: [NoAuthGuard]
  },
  {
    path: RouteNames.Register,
    loadComponent: () =>
      import('./modules/auth/register/register.component').then(m => m.RegisterComponent),
    canActivate: [NoAuthGuard]
  },
  {
    path: RouteNames.ManagerDashboard,
    loadComponent: () =>
      import('./modules/manager/manager-dashboard/manager-dashboard.component').then(m => m.ManagerDashboardComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Manager' }
  },
  {
    path: RouteNames.MechanicDashboard,
    loadComponent: () =>
      import('./modules/mechanic/mechanic-dashboard/mechanic-dashboard.component').then(m => m.MechanicDashboardComponent),
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Mechanic' }
  },
  { path: '**', redirectTo: RouteNames.Login }
];