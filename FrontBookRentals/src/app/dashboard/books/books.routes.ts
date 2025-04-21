import { Routes } from '@angular/router';

export const booksRoutes: Routes = [
  {
    path: '',
    children: [
      { path: '', loadComponent: () => import('./books.component').then(m => m.BooksComponent) },
      { path: ':id', loadComponent: () => import('./book-details/book-details.component').then(m => m.BookDetailsComponent) },
      { path: '', redirectTo: '', pathMatch: 'full' }
    ]
  }
]; 