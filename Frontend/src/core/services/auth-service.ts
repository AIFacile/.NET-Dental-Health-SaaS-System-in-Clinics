import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { DecodedToken, LoginRequest, LoginResponse, RoleName, UserSession } from '../../types/auth-types';
import { Observable, tap } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private http = inject(HttpClient);
  private readonly API_URL = 'https://localhost:7168/api/auth/login';
  private router = inject(Router);

  constructor() {
    const token = localStorage.getItem('id_token');
    if (token) {
      try {
        this.handleAuthentication(token);
      } catch (e) {
        this.logout();
      }
    }
  }

  currentUser = signal<UserSession | null>(null);

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(this.API_URL, credentials, { responseType: 'json'}).pipe(
      tap(response => {
        this.handleAuthentication(response.accessToken);
      })
    );
  }

  private handleAuthentication(token: string) {
    const decoded = jwtDecode<DecodedToken>(token);
    
    const user: UserSession = {
      username: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
      permissions: decoded.permissions,
      tenantId: decoded.tenant_id
    };

    this.currentUser.set(user);
    localStorage.setItem('id_token', token);
  }

  hasPermission(permission: string): boolean {
    return this.currentUser()?.permissions.includes(permission) ?? false;
  }

  hasRole(role: RoleName): boolean {
    return this.currentUser()?.role === role;
  }

  logout() {
    localStorage.removeItem('id_token');
    localStorage.removeItem('user_name');
    localStorage.removeItem('role');

    this.currentUser.set(null);

    this.router.navigate(['/login']).then(() => {
      console.log('User logged out and redirected to login page.')
    })
  }

  public isLoggedIn(): boolean {
    return !!localStorage.getItem('id_token');
  }
}
