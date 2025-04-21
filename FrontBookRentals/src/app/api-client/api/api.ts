export * from './auth.service';
import { AuthService } from './auth.service';
export * from './books.service';
import { BooksService } from './books.service';
export * from './clients.service';
import { ClientsService } from './clients.service';
export * from './orders.service';
import { OrdersService } from './orders.service';
export const APIS = [AuthService, BooksService, ClientsService, OrdersService];
