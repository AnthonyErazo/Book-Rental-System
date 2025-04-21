import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { BooksService } from '../../../core/services/books.service';
import { OrdersService } from '../../../core/services/orders.service';
import { BookResponseDto, OrderByBookResponseDto } from '../../../api-client';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-book-details',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule
  ],
  templateUrl: './book-details.component.html',
  styleUrls: ['./book-details.component.css']
})
export class BookDetailsComponent implements OnInit {
  book: BookResponseDto | null = null;
  orders: OrderByBookResponseDto[] = [];
  displayedColumns: string[] = ['client', 'rentalDate', 'status'];
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private booksService: BooksService,
    private ordersService: OrdersService
  ) {}

  ngOnInit() {
    const bookId = this.route.snapshot.paramMap.get('id');
    if (bookId) {
      this.loadBook(bookId);
    }
  }

  async loadBook(id: string) {
    this.isLoading = true;
    try {
      const [bookResponse, ordersResponse] = await Promise.all([
        this.booksService.getBookById(id),
        this.ordersService.getOrdersByIdBook(id)
      ]);

      if (bookResponse.success && bookResponse.data) {
        this.book = bookResponse.data;
      }

      if (ordersResponse.success && ordersResponse.data) {
        this.orders = ordersResponse.data;
      }
    } catch (error) {
      console.error('Error al cargar datos:', error);
    } finally {
      this.isLoading = false;
    }
  }

  openClientDetails(clientId: string) {
    this.router.navigate(['/dashboard/clients', clientId]);
  }

  goBack() {
    this.router.navigate(['/dashboard/books']);
  }
}
