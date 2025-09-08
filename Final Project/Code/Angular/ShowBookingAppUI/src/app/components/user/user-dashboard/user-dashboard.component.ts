import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserService, Movie, Seat } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-dashboard',
  templateUrl: './user-dashboard.component.html',
  styleUrls: ['./user-dashboard.component.css']
})
export class UserDashboardComponent implements OnInit {
  selectedSeats: number[] = [];
  selectedMovie?: Movie;
  showSeatModal: boolean = false;

  recommendedMovies: Movie[] = [];
  searchedMovies: Movie[] = [];

  loadingRecommended = true;
  loadingSearch = false;
  error: string | null = null;

  filters = {
    location: '',
    movieName: '',
    minPrice: null,
    maxPrice: null,
    showDate: '',
    startTime: '',
    endTime: '',
    language: ''
  };

  constructor(private router: Router, private userService: UserService) {}

  ngOnInit(): void {
    this.loadRecommendedMovies();
  }

  loadRecommendedMovies() {
    this.loadingRecommended = true;
    this.userService.getRecommendedMovies().subscribe({
      next: (res: Movie[]) => {
        console.log('Recommended movies:', res);
        this.recommendedMovies = res;
        this.loadingRecommended = false;
      },
      error: (err: any) => {
        console.error('Failed to load recommended movies', err);
        this.recommendedMovies = [];
        this.loadingRecommended = false;
      }
    });
  }

  searchMovies(): void {
  this.loadingSearch = true;
  this.searchedMovies = [];

  // Only send location and movieName
  const filterPayload = {
    location: this.filters.location,
    movieName: this.filters.movieName
  };

  this.userService.searchTheatres(filterPayload).subscribe({
    next: (res: any[]) => {
      // Flatten all theatre movies
      this.searchedMovies = res.flatMap(theatre => theatre.movies);
      this.loadingSearch = false;
    },
    error: (err: any) => {
      console.error('Filter failed', err);
      this.searchedMovies = [];
      this.loadingSearch = false;
    }
  });
}

  openSeatSelection(movie: Movie) {
    this.selectedMovie = movie;
    this.selectedSeats = [];
    this.showSeatModal = true;
    this.userService.getSeatsByMovie(movie.movieId, movie.theatre.theatreId).subscribe({
      next: seats => this.selectedMovie!.seats = seats,
      error: () => this.selectedMovie!.seats = []
    });
  }

  toggleSeatSelection(seat: Seat) {
    if (!seat.isAvailable) return;
    const index = this.selectedSeats.indexOf(seat.seatId);
    if (index > -1) this.selectedSeats.splice(index, 1);
    else this.selectedSeats.push(seat.seatId);
  }

  bookMovie(movie: Movie) {
  this.router.navigate(['/user/booking', movie.movieId, movie.theatre.theatreId]);
}

  clearFilters() {
    this.filters = { location: '', movieName: '', minPrice: null, maxPrice: null, showDate: '', startTime: '', endTime: '', language: '' };
    this.searchedMovies = [];
    console.log('Filters cleared');
  }
}
