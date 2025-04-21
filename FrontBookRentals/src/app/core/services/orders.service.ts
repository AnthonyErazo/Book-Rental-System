import { Injectable } from "@angular/core";
import { OrdersService as ApiOrdersService } from '../../api-client/api/orders.service';
import { firstValueFrom } from "rxjs";
import { BaseGenericResponse, BookResponseDtoICollectionGenericResponse, GuidGenericResponse, OrderByBookResponseDtoICollectionGenericResponse, OrderByClientResponseDtoICollectionGenericResponse, OrderRequestDto, OrderResponseDtoICollectionGenericResponse } from "../../api-client";

@Injectable({
    providedIn: 'root'
})
export class OrdersService {

    constructor(
        private apiOrdersService: ApiOrdersService
    ) {
    }

    async getOrders(): Promise<OrderResponseDtoICollectionGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersGet(
                    1,
                    10,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar obtener las ordenes'
            };
        }
    }
    async getOrderByDniClient(dni: string): Promise<BookResponseDtoICollectionGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersClientDniBooksGet(
                    dni,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar obtener los libros para el dni'
            };
        }
    }
    async changeOrderStatus(id: string): Promise<BaseGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersOrderIdStatusPatch(
                    id,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar cambiar el estado de la orden'
            };
        }
    }
    async deleteOrder(id: string): Promise<BaseGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersOrderIdDelete(
                    id,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar eliminar la orden'
            };
        }
    }
    async createOrder(order: OrderRequestDto): Promise<GuidGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersPost(
                    order,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar crear la orden'
            };
        }
    }
    async getOrdersByIdClient(clientId: string): Promise<OrderByClientResponseDtoICollectionGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersClientClientIdOrdersGet(
                    clientId,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar obtener la orden'
            };
        }
    }
    async getOrdersByIdBook(bookId: string): Promise<OrderByBookResponseDtoICollectionGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiOrdersService.apiOrdersBookBookIdOrdersGet(
                    bookId,
                    'body',
                    false,
                    { httpHeaderAccept: 'application/json' }
                )
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar obtener la orden'
            };
        }
    }
} 
