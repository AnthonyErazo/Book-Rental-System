import { Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';

export const authRoutes: Routes = [
  {
    path: '',
    canActivate: [AuthGuard],
    children: [
      { path: 'login', loadComponent: () => import('./login/login.component').then(m => m.LoginComponent) },
      { path: 'register', loadComponent: () => import('./register/register.component').then(m => m.RegisterComponent) },
      { path: '', redirectTo: 'login', pathMatch: 'full' }
    ]
  }
]; 