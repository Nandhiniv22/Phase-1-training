import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, map } from 'rxjs';
import { Seat } from '../models/user.model';

export interface Theatre {
  theatreId: number;
  name: string;
  location: string;
  movies?: Movie[];
}

export interface Movie {
  description: any;
  durationMinutes: any;
  language: any;
  movieId: number;
  title: string;
  screenType?: string;
  seatCategories: string[];
  bookings: number;
  showDate: string;
  showTime: string;
  theatre: Theatre;
  seats?: Seat[];
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'http://localhost:5227/api/user';

  constructor(private http: HttpClient) {}

  getRecommendedMovies(): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/recommended`).pipe(
      map(movies => movies.map(m => ({
        ...m,
        seatCategories: m.seatCategories || [],
        theatre: m.theatre || { theatreId: 0, name: 'Unknown', location: 'Unknown' }
      })))
    );
  }

  searchTheatres(filters: {
    location?: string;
    movieName?: string;
    minPrice?: number | null;
    maxPrice?: number | null;
    showDate?: string;
    startTime?: string;
    endTime?: string;
  }): Observable<Movie[]> {
    let params = new HttpParams();
    if (filters.location) params = params.set('location', filters.location);
    if (filters.movieName) params = params.set('movieName', filters.movieName);
    if (filters.minPrice != null) params = params.set('minPrice', filters.minPrice.toString());
    if (filters.maxPrice != null) params = params.set('maxPrice', filters.maxPrice.toString());
    if (filters.showDate) params = params.set('showDate', filters.showDate);
    if (filters.startTime) params = params.set('startTime', filters.startTime);
    if (filters.endTime) params = params.set('endTime', filters.endTime);

    return this.http.get<Movie[]>(`${this.apiUrl}/search`, { params }).pipe(
      map(movies => movies.map(m => ({
        ...m,
        seatCategories: m.seatCategories || [],
        theatre: m.theatre || { theatreId: 0, name: 'Unknown', location: 'Unknown' }
      })))
    );
  }

  getSeatsByMovie(movieId: number, theatreId: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(`${this.apiUrl}/movies/${movieId}/seats`);
  }

  bookSeats(movieId: number, theatreId: number, seatIds: number[]): Observable<any> {
    return this.http.post(`${this.apiUrl}/booking`, { movieId, theatreId, seatIds });
  }

  getTheatres(): Observable<Theatre[]> {
    return this.http.get<Theatre[]>(`${this.apiUrl}/theatres`);
  }

  getMovies(theatreId: number): Observable<Movie[]> {
    return this.http.get<Movie[]>(`${this.apiUrl}/theatre/${theatreId}/movies`).pipe(
      map(movies => movies.map(m => ({
        ...m,
        seatCategories: m.seatCategories || [],
        theatre: m.theatre || { theatreId, name: 'Unknown', location: 'Unknown' }
      })))
    );
  }

  getSeats(theatreId: number, movieId: number): Observable<Seat[]> {
    return this.http.get<Seat[]>(`${this.apiUrl}/theatre/${theatreId}/movie/${movieId}`);
  }
}

export { Seat };
