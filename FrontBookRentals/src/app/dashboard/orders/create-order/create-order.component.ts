import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../../../core/services/orders.service';
import { ClientsService } from '../../../core/services/clients.service';
import { BooksService } from '../../../core/services/books.service';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';
import { BookResponseDto, ClientResponseDto, OrderRequestDto } from '../../../api-client';

@Component({
  selector: 'app-create-order',
  standalone: true,
  imports: [
    CommonModule, 
    MatTableModule, 
    MatButtonModule, 
    MatIconModule,
    MatInputModule,
    MatFormFieldModule,
    FormsModule,
    MatCardModule
  ],
  templateUrl: './create-order.component.html',
  styleUrl: './create-order.component.css'
})
export class CreateOrderComponent implements OnInit {
  clients: ClientResponseDto[] = [];
  books: BookResponseDto[] = [];
  selectedClient: ClientResponseDto | null = null;
  selectedBooks: BookResponseDto[] = [];
  searchClient: string = '';
  searchBook: string = '';
  clientColumns: string[] = ['dni', 'name', 'age', 'select'];
  bookColumns: string[] = ['isbn','title', 'author', 'select'];

  constructor(
    private ordersService: OrdersService,
    private clientsService: ClientsService,
    private booksService: BooksService,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadClients();
    this.loadBooks();
  }

  async loadClients() {
    const response = await this.clientsService.getClients(1, 10, this.searchClient);
    if (response.success) {
      this.clients = response.data || [];
    }
  }

  async loadBooks() {
    const response = await this.booksService.getBooks(this.searchBook, 1, 10);
    if (response.success) {
      this.books = response.data || [];
    }
  }

  selectClient(client: any) {
    this.selectedClient = client;
  }

  selectBook(book: any) {
    if (!this.selectedBooks.some(b => b.bookId === book.bookId)) {
      this.selectedBooks = [book, ...this.selectedBooks];
    }
  }

  removeBook(book: any) {
    this.selectedBooks = this.selectedBooks.filter(b => b.bookId !== book.bookId);
  }

  isBookSelected(book: any): boolean {
    return this.selectedBooks.some(b => b.bookId === book.bookId);
  }

  toggleBookSelection(book: any) {
    if (this.isBookSelected(book)) {
      this.removeBook(book);
    } else {
      this.selectBook(book);
    }
  }

  async createOrder() {
    if (this.selectedClient && this.selectedBooks.length > 0) {
      const order: OrderRequestDto = {
        clientId: this.selectedClient.clientId,
        bookIds: this.selectedBooks.map(book => book.bookId)
      };
      
      const response = await this.ordersService.createOrder(order);
      if (response.success) {
        this.router.navigate(['/dashboard/orders']);
      }
    }
  }

  cancel() {
    this.router.navigate(['/dashboard/orders']);
  }
}
