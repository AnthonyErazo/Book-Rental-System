import { Injectable } from "@angular/core";
import { BooksService as ApiBooksService } from '../../api-client/api/books.service';
import { firstValueFrom } from "rxjs";
import { BaseGenericResponse, 
    BookResponseDtoICollectionGenericResponse, 
    BookRequestDto,
    BookResponseDtoGenericResponse, 
    BookUpdateRequestDto, 
    GuidGenericResponse } from "../../api-client";

@Injectable({
    providedIn: 'root'
})
export class BooksService {

    constructor(
        private apiBooksService: ApiBooksService
    ) {
    }

    async getBooks(search: string = '', page: number = 1, recordsPerPage: number = 10): Promise<BookResponseDtoICollectionGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiBooksService.apiBooksGet(
                    search,
                    page,
                    recordsPerPage,
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
                message: errorResponse.Message || 'Error al intentar obtener los libros'
            };
        }
    }
    async getBookById(id: string): Promise<BookResponseDtoGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiBooksService.apiBooksIdGet(
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
                message: errorResponse.Message || 'Error al intentar obtener el libro'
            };
        }
    }
    async patchBook(id: string, book: BookUpdateRequestDto): Promise<BaseGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiBooksService.apiBooksIdPatch(
                    id,
                    book,
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
                message: errorResponse.Message || 'Error al intentar obtener el libro'
            };
        }
    }
    async deleteBook(id: string): Promise<BaseGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiBooksService.apiBooksIdDelete(
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
                message: errorResponse.Message || 'Error al intentar eliminar el libro'
            };
        }
    }
    async createBook(book: BookRequestDto): Promise<GuidGenericResponse> {
        try {

            const response = await firstValueFrom(
                this.apiBooksService.apiBooksPost(
                    book,
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
                message: errorResponse.Message || 'Error al intentar eliminar el libro'
            };
        }
    }
} 