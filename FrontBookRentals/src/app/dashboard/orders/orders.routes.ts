import { Routes } from '@angular/router';

export const ordersRoutes: Routes = [
  {
    path: '',
    children: [
      { path: '', loadComponent: () => import('./orders.component').then(m => m.OrdersComponent) },
      { path: 'create', loadComponent: () => import('./create-order/create-order.component').then(m => m.CreateOrderComponent) },
      { path: '', redirectTo: '', pathMatch: 'full' }
    ]
  }
]; 