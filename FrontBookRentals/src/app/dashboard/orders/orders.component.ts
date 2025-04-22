import { Component, OnInit } from '@angular/core';
import { OrdersService } from '../../core/services/orders.service';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { OrderResponseDto } from '../../api-client';

@Component({
  selector: 'app-orders',
  standalone: true,
  imports: [
    CommonModule, 
    MatTableModule, 
    MatButtonModule, 
    MatIconModule,
    RouterModule,
    MatSlideToggleModule
  ],
  templateUrl: './orders.component.html',
  styleUrl: './orders.component.css'
})
export class OrdersComponent implements OnInit {
  orders: OrderResponseDto[] = [];
  displayedColumns: string[] = ['registerTime', 'client', 'book', 'status', 'actions'];

  constructor(private ordersService: OrdersService) {}

  ngOnInit() {
    this.loadOrders();
  }

  async loadOrders() {
    const response = await this.ordersService.getOrders();
    if (response.success) {
      this.orders = response.data || [];
    }
  }

  async deleteOrder(id: string) {
    const response = await this.ordersService.deleteOrder(id);
    if (response.success) {
      this.loadOrders();
    }
  }

  async toggleOrderStatus(id: string) {
    const response = await this.ordersService.changeOrderStatus(id);
    if (response.success) {
      this.loadOrders();
    }
  }
}
