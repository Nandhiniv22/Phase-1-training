import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrganizerService {
  private apiUrl = 'http://localhost:5227/api/organizer';

  constructor(private http: HttpClient) {}

  createTheatre(theatre: any): Observable<any> {
    return this.http.post(
      `${this.apiUrl}/theatre/${theatre.organizerId}`,
      {
        name: theatre.name,
        location: theatre.location
      }
    );
  }
  
  getMyTheatres(organizerId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/my-theatres/${organizerId}`);
  }

  addMovie(theatreId: number, movie: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/theatre/${theatreId}/movie`, movie);
  }

  // ✅ Add seats for a movie
  addSeats(movieId: number, seats: any[]): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/movies/${movieId}/seats`, seats);
  }

  getBookings(theatreId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/theatre/${theatreId}/bookings`);
  }

  getSeats(theatreId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/theatre/${theatreId}/seats`);
  }

  getMoviesByTheatre(theatreId: number): Observable<any[]> {
  return this.http.get<any[]>(`${this.apiUrl}/theatre/${theatreId}/movies`);
}

deleteMovie(movieId: number): Observable<any> {
  return this.http.delete(`${this.apiUrl}/movie/${movieId}`);
}

getMovieById(movieId: number): Observable<any> {
  return this.http.get(`${this.apiUrl}/movie/${movieId}`);
}

updateMovie(movieId: number, movie: any): Observable<any> {
  return this.http.put(`${this.apiUrl}/movie/${movieId}`, movie);
}

getBookingsByTheatre(theatreId: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiUrl}/theatre/${theatreId}/bookings`);
  }
  
}
