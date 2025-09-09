import { Component, OnInit } from '@angular/core';
import { OrganizerService } from '../../../services/organizer.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-organizer-dashboard',
  templateUrl: './organizer-dashboard.component.html',
  styleUrls: ['./organizer-dashboard.component.css']
})
export class OrganizerDashboardComponent implements OnInit {
  theatres: any[] = [];
  userId: number = 0;
  moviesByTheatre: { [key: number]: any[] } = {};

  searchText: string = '';
  selectedLocation: string = '';

  selectedTheatreId: number | null = null;

  bookingsByTheatre: { [key: number]: any[] } = {};
  selectedBookingsTheatreId: number | null = null;

  constructor(private organizerService: OrganizerService, private router : Router) {}

  ngOnInit(): void {
    const uid = localStorage.getItem('userId');
    if (uid) {
      this.userId = Number(uid);
      this.loadTheatres();
    }
  }

  loadTheatres() {
    this.organizerService.getMyTheatres(this.userId).subscribe({
      next: (data) => {
        this.theatres = data;
      },
      error: () => {
        console.error('Failed to load theatres.');
      }
    });
  }

  filteredTheatres() {
    return this.theatres.filter(t =>
      (!this.searchText || t.name.toLowerCase().includes(this.searchText.toLowerCase())) &&
      (!this.selectedLocation || t.location === this.selectedLocation)
    );
  }
  loadMovies(theatreId: number) {
  if (this.selectedTheatreId === theatreId) {
    // Close if already open
    this.selectedTheatreId = null;
  } else {
    this.organizerService.getMoviesByTheatre(theatreId).subscribe({
      next: (data) => {
        this.moviesByTheatre[theatreId] = data;
        this.selectedTheatreId = theatreId; // Open this theatre
      },
      error: () => {
        console.error('Failed to load movies');
      }
    });
  }
}

deleteMovie(movieId: number, theatreId: number) {
  if (confirm('Are you sure you want to delete this movie?')) {
    this.organizerService.deleteMovie(movieId).subscribe({
      next: () => {
        this.loadMovies(theatreId); // reload after delete
      },
      error: () => {
        console.error('Failed to delete movie');
      }
    });
  }
}

editMovie(movieId: number) {
  this.router.navigate(['/organizer/edit-movie', movieId]);
}

loadBookings(theatreId: number) {
    if (this.selectedBookingsTheatreId === theatreId) {
      this.selectedBookingsTheatreId = null;
    } else {
      this.organizerService.getBookingsByTheatre(theatreId).subscribe({
        next: (data) => {
          this.bookingsByTheatre[theatreId] = data;
          this.selectedBookingsTheatreId = theatreId;
        },
        error: () => {
          console.error('Failed to load bookings');
        }
      });
    }
  }

}
