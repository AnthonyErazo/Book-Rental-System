import { Component, OnInit, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatDialog } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { BooksService } from '../../core/services/books.service';
import { BookResponseDto } from '../../api-client';
import { ModalFormComponent } from '../../shared/components/modal-form/modal-form.component';

@Component({
  selector: 'app-books',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatPaginatorModule
  ],
  templateUrl: './books.component.html',
  styleUrls: ['./books.component.css']
})
export class BooksComponent implements OnInit {
  displayedColumns: string[] = ['name', 'author', 'isbn', 'status', 'actions'];
  dataSource: BookResponseDto[] = [];
  isLoading = false;
  totalItems = 0;
  pageSize = 10;
  currentPage = 0;

  constructor(
    private booksService: BooksService,
    private dialog: MatDialog,
    private router: Router
  ) {}

  ngOnInit() {
    this.loadBooks();
  }

  async loadBooks() {
    this.isLoading = true;
    try {
      const response = await this.booksService.getBooks();
      if (response.success && response.data) {
        this.dataSource = response.data;
        this.totalItems = response.data.length;
      }
    } catch (error) {
      console.error('Error al cargar libros:', error);
    } finally {
      this.isLoading = false;
    }
  }

  onPageChange(event: PageEvent) {
    this.currentPage = event.pageIndex;
    this.pageSize = event.pageSize;
    this.loadBooks();
  }

  openNewBookModal() {
    const dialogRef = this.dialog.open(ModalFormComponent, {
      width: '500px',
      data: {
        title: 'Nuevo Libro',
        fields: [
          { name: 'name', label: 'Título', type: 'text', required: true },
          { name: 'author', label: 'Autor', type: 'text', required: true },
          { name: 'isbn', label: 'ISBN', type: 'text', required: true }
        ]
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.createBook(result);
      }
    });
  }

  openEditBookModal(book: BookResponseDto) {
    const dialogRef = this.dialog.open(ModalFormComponent, {
      width: '500px',
      data: {
        title: 'Editar Libro',
        fields: [
          { name: 'name', label: 'Título', type: 'text', required: true, value: book.name },
          { name: 'author', label: 'Autor', type: 'text', required: true, value: book.author },
          { name: 'isbn', label: 'ISBN', type: 'text', required: true, value: book.isbn }
        ]
      }
    });

    dialogRef.afterClosed().subscribe(result => {
      if (result) {
        this.updateBook(book.bookId, result);
      }
    });
  }

  async createBook(bookData: any) {
    try {
      const response = await this.booksService.createBook(bookData);
      if (response.success) {
        this.loadBooks();
      }
    } catch (error) {
      console.error('Error al crear libro:', error);
    }
  }

  async updateBook(id: string, bookData: any) {
    try {
      const response = await this.booksService.patchBook(id, bookData);
      if (response.success) {
        this.loadBooks();
      }
    } catch (error) {
      console.error('Error al actualizar libro:', error);
    }
  }

  async deleteBook(id: string) {
    if (confirm('¿Estás seguro de eliminar este libro?')) {
      try {
        const response = await this.booksService.deleteBook(id);
        if (response.success) {
          this.loadBooks();
        }
      } catch (error) {
        console.error('Error al eliminar libro:', error);
      }
    }
  }

  openBookDetails(book: BookResponseDto) {
    this.router.navigate(['/dashboard/books', book.bookId]);
  }
}
