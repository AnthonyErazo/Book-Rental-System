import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatTableModule } from '@angular/material/table';
import { ClientsService } from '../../../core/services/clients.service';
import { OrdersService } from '../../../core/services/orders.service';
import { ClientResponseDto, OrderByClientResponseDto } from '../../../api-client';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-client-details',
  standalone: true,
  imports: [
    CommonModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatTableModule
  ],
  templateUrl: './client-details.component.html',
  styleUrls: ['./client-details.component.css']
})
export class ClientDetailsComponent implements OnInit {
  client: ClientResponseDto | null = null;
  orders: OrderByClientResponseDto[] = [];
  displayedColumns: string[] = ['rentalDate', 'books', 'status'];
  isLoading = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private clientsService: ClientsService,
    private ordersService: OrdersService
  ) {}

  ngOnInit() {
    const clientId = this.route.snapshot.paramMap.get('id');
    if (clientId) {
      this.loadClient(clientId);
    }
  }

  async loadClient(id: string) {
    this.isLoading = true;
    try {
      const [clientResponse, ordersResponse] = await Promise.all([
        this.clientsService.getClientById(id),
        this.ordersService.getOrdersByIdClient(id)
      ]);

      if (clientResponse.success && clientResponse.data) {
        this.client = clientResponse.data;
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

  openBookDetails(bookId: string) {
    this.router.navigate(['/dashboard/books', bookId]);
  }

  goBack() {
    this.router.navigate(['/dashboard/clients']);
  }
}
