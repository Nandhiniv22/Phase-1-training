import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-edit-movie',
  templateUrl: './edit-movie.component.html',
})
export class EditMovieComponent implements OnInit {
  theatreId!: number;
  movieId!: number;
  movie: any = {
    title: '',
    language: '',
    description: '',
    durationMinutes: 0,
    screenType: '',
    seatCategories: []
  };

  constructor(
    private route: ActivatedRoute,
    private http: HttpClient,
    private router: Router
  ) {}

  ngOnInit(): void {
    const theatreParam = this.route.snapshot.paramMap.get('theatreId');
    const movieParam = this.route.snapshot.paramMap.get('movieId');

    this.theatreId = theatreParam ? Number(theatreParam) : 0;
    this.movieId = movieParam ? Number(movieParam) : 0;

    if (!this.theatreId || !this.movieId) {
      alert('Invalid theatre or movie ID.');
      this.router.navigate(['/organizer/dashboard']);
      return;
    }

    // Load existing movie data
    this.http.get(`http://localhost:5227/api/organizer/movie/${this.movieId}`)
      .subscribe({
        next: (data: any) => this.movie = data,
        error: (err) => {
          console.error(err);
          alert('Failed to load movie');
          this.router.navigate([`/organizer/theatre/${this.theatreId}/movies`]);
        }
      });
  }

  onSubmit() {
    this.movie.durationMinutes = Number(this.movie.durationMinutes);
    if (this.movie.seatCategories && !Array.isArray(this.movie.seatCategories)) {
      this.movie.seatCategories = [this.movie.seatCategories];
    }

    this.http.put(`http://localhost:5227/api/organizer/movie/${this.movieId}`, this.movie)
      .subscribe({
        next: () => {
          alert('Movie updated successfully');
          this.router.navigate([`/organizer/theatre/${this.theatreId}/movies`]);
        },
        error: (err) => {
          console.error(err);
          alert('Failed to update movie');
        }
      });
  }
}
