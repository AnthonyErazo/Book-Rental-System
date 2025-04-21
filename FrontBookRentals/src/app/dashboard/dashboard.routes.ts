import { Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { DashboardComponent } from './dashboard.component';

export const dashboardRoutes: Routes = [
  {
    path: '',
    component: DashboardComponent,
    canActivate: [AuthGuard],
    children: [
      { path: '', redirectTo: 'clients', pathMatch: 'full' },
      { 
        path: 'clients',
        loadChildren: () => import('./clients/clients.routes').then(m => m.clientsRoutes)
      },
      { 
        path: 'orders',
        loadChildren: () => import('./orders/orders.routes').then(m => m.ordersRoutes)
      },
      { 
        path: 'books',
        loadChildren: () => import('./books/books.routes').then(m => m.booksRoutes)
      }
    ]
  }
]; 