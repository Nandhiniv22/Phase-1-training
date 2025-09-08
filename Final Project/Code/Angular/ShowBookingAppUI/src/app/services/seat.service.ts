import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface SeatFromApi {
  SeatId: number;
  SeatNumber: string;
  SeatType: 'Premium' | 'Gold' | 'Regular';
  Price: number;
  IsAvailable: boolean;
  TheatreId: number;
  MovieId: number;
}

@Injectable({
  providedIn: 'root'
})
export class SeatService {
  private apiUrl = 'http://localhost:5000/api/seats'; // replace with your backend API

  constructor(private http: HttpClient) {}

  getSeats(): Observable<SeatFromApi[]> {
    return this.http.get<SeatFromApi[]>(this.apiUrl);
  }
}
