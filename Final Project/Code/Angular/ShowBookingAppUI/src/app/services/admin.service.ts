import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  private apiUrl = 'http://localhost:5227/api/admin';

  constructor(private http: HttpClient) {}

  // Users
  getUsers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/users`);
  }

  blockUser(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/block/${userId}`, {});
  }

  unblockUser(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/unblock/${userId}`, {});
  }

  // Organizers
  getOrganizers(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/organizers`);
  }

  approveOrganizer(userId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/approve-organizer/${userId}`, {});
  }

  removeOrganizer(userId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/remove-organizer/${userId}`);
  }

  getApprovedOrganizers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/approved-organizers`);
  }

  getUnapprovedOrganizers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/unapproved-organizers`);
  }

  getStatistics(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/statistics`);
  }

  // New method to get movie statistics
  getMovieStatistics(): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}/movie-statistics`);
  }
}
