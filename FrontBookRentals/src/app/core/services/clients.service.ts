import { Injectable } from "@angular/core";
import { ClientsService as ApiClientsService } from '../../api-client/api/clients.service';
import { firstValueFrom } from "rxjs";
import {
    BaseGenericResponse,
    ClientRequestDto,
    ClientResponseDtoGenericResponse,
    ClientResponseDtoICollectionGenericResponse,
    ClientUpdateRequestDto,
    GuidGenericResponse
} from "../../api-client";

@Injectable({
    providedIn: 'root'
})
export class ClientsService {

    constructor(
        private apiClientsService: ApiClientsService
    ) {
    }


    async getClients(page: number = 1, recordsPerPage: number = 10): Promise<ClientResponseDtoICollectionGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiClientsService.apiClientsGet('', page, recordsPerPage)
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar obtener los clientes'
            };
        }
    }
    async getClientById(id: string): Promise<ClientResponseDtoGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiClientsService.apiClientsIdGet(id)
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar obtener el cliente'
            };
        }
    }
    async patchClient(id: string, client: ClientUpdateRequestDto): Promise<BaseGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiClientsService.apiClientsIdPatch(
                    id,
                    client,
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
                message: errorResponse.Message || 'Error al intentar actualizar el cliente'
            };
        }
    }
    async deleteClient(id: string): Promise<BaseGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiClientsService.apiClientsIdDelete(id)
            );
            return response;
        } catch (error: any) {
            console.error('Error en registro:', error);
            const errorResponse = error.error || {};
            return {
                success: errorResponse.Success === false ? false : false,
                message: errorResponse.Message || 'Error al intentar eliminar el cliente'
            };
        }
    }
    async createClient(client: ClientRequestDto): Promise<GuidGenericResponse> {
        try {
            const response = await firstValueFrom(
                this.apiClientsService.apiClientsPost(
                    client,
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
                message: errorResponse.Message || 'Error al intentar crear el cliente'
            };
        }
    }
} 