import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private apiUrl = 'http://localhost:5227/api/booking';

  constructor(private http: HttpClient) {}

  createBooking(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, data);
  }

  getUserBookings(userId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/my-bookings?userId=${userId}`);
  }

  cancelBooking(bookingId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/cancel/${bookingId}`, {});
  }
}
