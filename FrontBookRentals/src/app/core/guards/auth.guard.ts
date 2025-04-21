import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    const isLoggedIn = this.authService.isLoggedIn();
    const currentUrl = state.url;

    if (isLoggedIn && currentUrl.startsWith('/auth')) {
      this.router.navigate(['/dashboard']);
      return false;
    }

    if (!isLoggedIn && currentUrl.startsWith('/dashboard')) {
      this.router.navigate(['/auth/login']);
      return false;
    }

    return true;
  }
} 