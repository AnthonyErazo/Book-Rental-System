import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatTableModule } from '@angular/material/table';
import { MatCardModule } from '@angular/material/card';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { OrdersService } from '../../core/services/orders.service';
import { ClientsService } from '../../core/services/clients.service';

@Component({
  selector: 'app-client-books-search',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatTableModule,
    MatCardModule,
    MatSnackBarModule
  ],
  templateUrl: './client-books-search.component.html',
  styleUrls: ['./client-books-search.component.css']
})
export class ClientBooksSearchComponent {
  searchDni: string = '';
  clientBooks: any[] = [];
  clientInfo: any = null;
  displayedColumns: string[] = ['title', 'author', 'isbn'];
  hasSearched: boolean = false;

  constructor(
    private ordersService: OrdersService,
    private clientsService: ClientsService,
    private snackBar: MatSnackBar
  ) {}

  validateDni(dni: string): boolean {
    return /^\d{8}$/.test(dni);
  }

  async searchClientBooks() {
    if (!this.searchDni) {
      this.snackBar.open('Por favor ingrese un DNI', 'Cerrar', { duration: 3000 });
      return;
    }

    if (!this.validateDni(this.searchDni)) {
      this.snackBar.open('El DNI debe contener exactamente 8 dígitos numéricos', 'Cerrar', { duration: 3000 });
      return;
    }

    this.hasSearched = true;
    try {
      // Buscar información del cliente
      const clientResponse = await this.clientsService.getClients(1, 1, this.searchDni);
      if (clientResponse.success && clientResponse.data && clientResponse.data.length > 0) {
        this.clientInfo = clientResponse.data[0];
      } else {
        this.clientInfo = null;
        this.snackBar.open('No se encontró el cliente', 'Cerrar', { duration: 3000 });
        return;
      }

      // Buscar libros del cliente
      const booksResponse = await this.ordersService.getOrderByDniClient(this.searchDni);
      if (booksResponse.success) {
        this.clientBooks = booksResponse.data || [];
      }
    } catch (error) {
      console.error('Error al buscar información:', error);
      this.clientBooks = [];
      this.clientInfo = null;
      this.snackBar.open('Error al buscar la información', 'Cerrar', { duration: 3000 });
    }
  }
}
