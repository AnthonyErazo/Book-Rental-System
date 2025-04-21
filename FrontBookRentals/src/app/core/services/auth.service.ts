import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Router } from '@angular/router';
import { AuthService as ApiAuthService } from '../../api-client/api/auth.service';
import { firstValueFrom } from 'rxjs';
import { AuthRequestDto,
  BaseGenericResponse,
  LoginResponseDtoGenericResponse } from '../../api-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private isAuthenticated = new BehaviorSubject<boolean>(false);
  private readonly TOKEN_KEY = 'auth_token';
  private readonly EXPIRATION_KEY = 'token_expiration';

  constructor(
    private router: Router,
    private apiAuthService: ApiAuthService
  ) {
    this.initializeAuthState();
  }

  private initializeAuthState(): void {
    const token = this.getToken();
    const expiration = this.getTokenExpiration();
    
    if (token && expiration) {
      const expirationDate = new Date(expiration);
      if (expirationDate > new Date()) {
        this.isAuthenticated.next(true);
      } else {
        this.clearToken();
      }
    }
  }

  private saveToken(token: string, expiration: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
    localStorage.setItem(this.EXPIRATION_KEY, expiration);
    this.isAuthenticated.next(true);
  }

  public clearToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
    localStorage.removeItem(this.EXPIRATION_KEY);
    this.isAuthenticated.next(false);
  }

  getToken(): string | null {
    const token = localStorage.getItem(this.TOKEN_KEY);
    const expiration = this.getTokenExpiration();
  
    if (!token || !expiration) return null;
  
    const expirationDate = new Date(expiration);
    if (expirationDate < new Date()) {
      this.clearToken();
      return null;
    }
  
    return token;
  }
  

  getTokenExpiration(): string | null {
    return localStorage.getItem(this.EXPIRATION_KEY);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    const expiration = this.getTokenExpiration();
    
    if (token && expiration) {
      const expirationDate = new Date(expiration);
      return expirationDate > new Date();
    }
    return false;
  }

  async login(username: string, password: string): Promise<LoginResponseDtoGenericResponse> {
    try {
      const loginRequest: AuthRequestDto = {
        userName: username,
        password: password
      };
      
      const response = await firstValueFrom(
        this.apiAuthService.apiAuthLoginPost(
          loginRequest,
          'body',
          false,
          { httpHeaderAccept: 'application/json' }
        )
      );

      if (response.success && response.data?.token) {
        this.saveToken(response.data.token, response.data.expirationDate);
        this.router.navigate(['/dashboard']);
      }
      return response;
    } catch (error: any) {
      console.error('Error en registro:', error);
      const errorResponse = error.error || {};
      return {
        success: errorResponse.Success === false ? false : false,
        message: errorResponse.Message || 'Error al intentar registrar'
      };
    }
  }

  async register(username: string, password: string): Promise<BaseGenericResponse> {
    try {
      const registerRequest: AuthRequestDto = {
        userName: username,
        password: password
      };
      
      const response = await firstValueFrom(
        this.apiAuthService.apiAuthRegisterPost(
          registerRequest,
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
        message: errorResponse.Message || 'Error al intentar registrar'
      };
    }
  }

  async logoutSession(): Promise<BaseGenericResponse> {
    const response = await firstValueFrom(
      this.apiAuthService.apiAuthLogoutPost(
        'body',
        false,
        { httpHeaderAccept: 'application/json' }
      )
    );
    this.clearToken();
    this.router.navigate(['/auth/login']);
    return response;
  }

  async logoutAllSessions(): Promise<BaseGenericResponse> {
    const response = await firstValueFrom(
      this.apiAuthService.apiAuthLogoutAllPost(
        'body',
        false,
        { httpHeaderAccept: 'application/json' }
      )
    );
    this.clearToken();
    this.router.navigate(['/auth/login']);
    return response;
  }
} 