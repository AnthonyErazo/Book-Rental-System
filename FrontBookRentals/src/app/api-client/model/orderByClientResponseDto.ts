/**
 * Book Rentals API
 *
 * 
 *
 * NOTE: This class is auto generated by OpenAPI Generator (https://openapi-generator.tech).
 * https://openapi-generator.tech
 * Do not edit the class manually.
 */
import { BookResponseDto } from './bookResponseDto';


export interface OrderByClientResponseDto { 
    id: string;
    registerTime: string;
    status: boolean;
    books: Array<BookResponseDto>;
}

