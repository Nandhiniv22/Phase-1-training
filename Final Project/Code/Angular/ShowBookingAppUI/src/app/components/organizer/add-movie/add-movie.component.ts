import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrganizerService } from '../../../services/organizer.service';

@Component({
  selector: 'app-add-movie',
  templateUrl: './add-movie.component.html',
  styleUrls: ['./add-movie.component.css']
})
export class AddMovieComponent implements OnInit {
  theatreId!: number;
  message: string = '';
  isSuccess: boolean = false;
  today: string = '';

  movie = {
  title: '',
  language: '',
  description: '',
  durationMinutes: 120,
  screenType: '2D',
  showDate: '',
  showTime: ''
};

  seatCategories = [
    { name: 'Premium', price: 400, rows: 0 },
    { name: 'Gold', price: 250, rows: 0 },
    { name: 'Regular', price: 150, rows: 0 }
  ];

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private organizerService: OrganizerService
  ) {}

ngOnInit(): void {
  this.theatreId = Number(this.route.snapshot.paramMap.get('id'));

  const now = new Date();
  this.today = now.toISOString().split('T')[0]; 
}

getMinTime(): string | null {
  if (!this.movie?.showDate) return null;

  if (this.movie.showDate === this.today) {
    const now = new Date();
    return now.toTimeString().slice(0, 5); 
  }

  return null; 
}

  onSubmit() {
  if (!this.movie.title || !this.movie.language || !this.movie.description || !this.movie.showDate || !this.movie.showTime) {
    this.message = 'Please fill in all movie details including show date and time.';
    this.isSuccess = false;
    return;
  }

  this.organizerService.addMovie(this.theatreId, this.movie).subscribe({
    next: (movieRes) => {
      const movieId = movieRes.movieId;

      // Generate seats based on user input
      const seats: any[] = [];
      let currentRow = 1;
      this.seatCategories.forEach(cat => {
        for (let r = 1; r <= cat.rows; r++) {
          for (let c = 1; c <= 10; c++) {
            seats.push({
              SeatNumber: `${String.fromCharCode(64 + currentRow)}${c}`,
              SeatType: cat.name,
              Price: cat.price,
              TheatreId: this.theatreId,
              IsAvailable: true
            });
          }
          currentRow++;
        }
      });

      this.organizerService.addSeats(movieId, seats).subscribe({
        next: () => {
          this.message = 'Movie, showtime, and seats added successfully!';
          this.isSuccess = true;
        },
        error: () => {
          this.message = 'Movie and showtime added, but failed to add seats.';
          this.isSuccess = false;
        }
      });
    },
    error: () => {
      this.message = 'Failed to add movie.';
      this.isSuccess = false;
    }
  });
}
}