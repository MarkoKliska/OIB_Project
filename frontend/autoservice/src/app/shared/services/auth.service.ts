import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

import { jwtDecode } from 'jwt-decode';
import { environment } from '../environments/environment';
import { CreateUserResponseDto, LoginResponseDto } from '../dto/responses/auth.response.tdo';
import { CreateUserRequestDto, LoginRequestDto } from '../dto/requests/auth.request.tdo';

interface JwtPayload {
  exp: number;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
  FullName?: string;
  unique_name?: string;
  nameid?: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly TOKEN_KEY = 'token';
  private readonly api = `${environment.apiUrl}/auth`;

  constructor(private http: HttpClient) {}

  // ==================== HTTP ====================

  login(request: LoginRequestDto): Observable<LoginResponseDto> {
    return this.http.post<LoginResponseDto>(`${this.api}/login`, request).pipe(
      tap(res => this.setToken(res.token))
    );
  }

  register(request: CreateUserRequestDto): Observable<CreateUserResponseDto> {
    return this.http.post<CreateUserResponseDto>(`${this.api}/register`, request).pipe(
      tap(res => this.setToken(res.token))
    );
  }

  // ==================== TOKEN ====================

  getToken(): string | null {
    return localStorage.getItem(this.TOKEN_KEY);
  }

  setToken(token: string): void {
    localStorage.setItem(this.TOKEN_KEY, token);
  }

  removeToken(): void {
    localStorage.removeItem(this.TOKEN_KEY);
  }

  logout(): void {
    this.removeToken();
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    return !this.isTokenExpired(token);
  }

  private isTokenExpired(token: string): boolean {
    try {
      const payload = jwtDecode<JwtPayload>(token);
      return Date.now() > payload.exp * 1000;
    } catch {
      return true;
    }
  }

  // ==================== CLAIMS ====================

  private getPayload(): JwtPayload | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      return jwtDecode<JwtPayload>(token);
    } catch {
      return null;
    }
  }

  getUserRole(): string | null {
    const payload = this.getPayload();
    if (!payload) return null;
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
  }

  getFullName(): string | null {
    const payload = this.getPayload();
    return payload?.FullName ?? payload?.unique_name ?? null;
  }

  isManager(): boolean {
    return this.getUserRole() === 'Manager';
  }

  isMechanic(): boolean {
    return this.getUserRole() === 'Mechanic';
  }
}