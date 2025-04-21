import { Routes } from '@angular/router';

export const ordersRoutes: Routes = [
  {
    path: '',
    children: [
      { path: '', loadComponent: () => import('./orders.component').then(m => m.OrdersComponent) },
      { path: '', redirectTo: '', pathMatch: 'full' }
    ]
  }
]; 