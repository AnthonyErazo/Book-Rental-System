import { ApplicationConfig, importProvidersFrom, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimations } from '@angular/platform-browser/animations';
import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { ApiModule, Configuration } from './api-client';
import { authInterceptor } from './core/interceptors/auth.interceptor';

const backendUrl = 'http://localhost:5000';

export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideAnimations(),
    provideHttpClient(
      withInterceptors([authInterceptor])
    ),
    importProvidersFrom(ApiModule.forRoot(() => new Configuration({
      basePath: backendUrl,
      accessToken: () => {
        const token = localStorage.getItem('auth_token');
        const expiration = localStorage.getItem('token_expiration');

        if (!token || !expiration) return '';

        const expiresAt = new Date(expiration);
        const now = new Date();

        if (expiresAt <= now) {
          localStorage.removeItem('auth_token');
          localStorage.removeItem('token_expiration');
          return '';
        }

        return token;
      }
    })))
  ]
};
