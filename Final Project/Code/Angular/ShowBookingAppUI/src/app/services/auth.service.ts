// auth.service.ts
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'http://localhost:5227/api/auth';

  constructor(private http: HttpClient, private router: Router) {}

  login(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, data);
  }

  register(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, data);
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('role');
  }

  setToken(token: string, role: string) {
    localStorage.setItem('token', token);
    localStorage.setItem('role', role);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }

  isAdmin(): boolean {
    return localStorage.getItem('role') === 'Admin';
  }

  isUser(): boolean {
    return localStorage.getItem('role') === 'User';
  }

  getProfile(): Observable<any> {
  return this.http.get(`${this.apiUrl}/profile`);
  }

  updateProfile(data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/profile`, data);
  }

  // auth.service.ts (additions only)
  getUserRole(): string | null {
    // prefer an explicitly stored role (set at login). Fallback: try decode token.
    const role = localStorage.getItem('role');
    if (role) return role;
    return this.getRoleFromToken();
  }

  private getRoleFromToken(): string | null {
    const token = this.getToken();
    if (!token) return null;
    try {
      const payloadBase64 = token.split('.')[1];
      if (!payloadBase64) return null;
      // atob is available in browser to decode base64
      const payloadJson = decodeURIComponent(
        Array.prototype.map
          .call(atob(payloadBase64), (c: string) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
      const payload = JSON.parse(payloadJson);
      // adapt these keys depending on how your backend embeds role(s)
      return payload.role || payload.roles || payload.Rol || null;
    } catch (e) {
      return null;
    }
  }

 getLoggedInUserId(): number {
  const userId = localStorage.getItem('userId');
  return userId ? Number(userId) : 0;
}

}
