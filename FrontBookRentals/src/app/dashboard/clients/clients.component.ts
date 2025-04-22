import { Component, OnInit } from '@angular/core';
import { ClientsService } from '../../core/services/clients.service';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatDialog } from '@angular/material/dialog';
import { ModalFormComponent } from '../../shared/components/modal-form/modal-form.component';
import { Router } from '@angular/router';
import { ClientRequestDto, ClientResponseDto, ClientUpdateRequestDto } from '../../api-client';

@Component({
  selector: 'app-clients',
  templateUrl: './clients.component.html',
  styleUrls: ['./clients.component.css'],
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ]
})
export class ClientsComponent implements OnInit {
  displayedColumns: string[] = ['names', 'lastNames', 'dni', 'age', 'actions'];
  dataSource: ClientResponseDto[] = [];
  totalItems = 0;
  pageSize = 10;
  currentPage = 0;
  isLoading = false;

  constructor(
    private clientsService: ClientsService,
    private dialog: MatDialog,
    private router: Router
  ) { }

  ngOnInit() {
    this.loadClients();
  }

  openNewClientModal() {
    const dialogRef = this.dialog.open(ModalFormComponent, {
      width: '500px',
      data: {
        title: 'Nuevo Cliente',
        fields: [
          { name: 'names', label: 'Nombres', type: 'text' },
          { name: 'lastNames', label: 'Apellidos', type: 'text' },
          { name: 'dni', label: 'DNI', type: 'text' },
          { name: 'age', label: 'Edad', type: 'number' }
        ]
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.createClient(result);
      }
    });
  }

  openEditClientModal(id: string, client: ClientResponseDto) {
    const dialogRef = this.dialog.open(ModalFormComponent, {
      width: '500px',
      data: {
        title: 'Editar Cliente',
        fields: [
          { name: 'names', label: 'Nombres', type: 'text' },
          { name: 'lastNames', label: 'Apellidos', type: 'text' },
          { name: 'dni', label: 'DNI', type: 'text' },
          { name: 'age', label: 'Edad', type: 'number' }
        ],
        initialData: client
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateClient(id, result);
      }
    });
  }

  openClientDetails(client: ClientResponseDto) {
    this.router.navigate(['/dashboard/clients', client.clientId]);
  }

  async deleteClient(id: string) {
    const response = await this.clientsService.deleteClient(id);
    if (response.success) {
      this.loadClients();
    }
  }

  async createClient(clientData: ClientRequestDto) {
    const response = await this.clientsService.createClient(clientData);
    if (response.success) {
      this.loadClients();
    }
  }

  async updateClient(id: string, clientData: ClientUpdateRequestDto) {
    const response = await this.clientsService.patchClient(id, clientData);
    if (response.success) {
      this.loadClients();
    }
  }

  async loadClients() {
    this.isLoading = true;
    const response = await this.clientsService.getClients(this.currentPage + 1, this.pageSize);
    console.log(response);
    if (response.success) {
      this.dataSource = response.data || [];
      this.totalItems = response.data?.length || 0;
    }
    this.isLoading = false;
  }

  onPageChange(event: PageEvent) {
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadClients();
  }
}
