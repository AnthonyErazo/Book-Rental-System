import { Routes } from '@angular/router';

export const clientsRoutes: Routes = [
  {
    path: '',
    children: [
      { path: '', loadComponent: () => import('./clients.component').then(m => m.ClientsComponent) },
      { path: ':id', loadComponent: () => import('./client-details/client-details.component').then(m => m.ClientDetailsComponent) },
      { path: '', redirectTo: '', pathMatch: 'full' }
    ]
  }
]; 