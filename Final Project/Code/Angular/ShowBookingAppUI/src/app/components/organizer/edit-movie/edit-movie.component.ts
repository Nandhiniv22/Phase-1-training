import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { OrganizerService } from 'src/app/services/organizer.service';

@Component({
  selector: 'app-edit-movie',
  templateUrl: './edit-movie.component.html',
  styleUrls: ['./edit-movie.component.css']
})
export class EditMovieComponent implements OnInit {
  movieId!: number;
  movie: any = {
    title: '',
    language: '',
    description: '',
    durationMinutes: 0,
    screenType: '2D',
    showDate: '',
    showTime: ''
  };
  message = '';
  today: string = new Date().toISOString().split('T')[0];  // ✅ declare today

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private organizerService: OrganizerService
  ) {}

  ngOnInit(): void {
  this.movieId = Number(this.route.snapshot.paramMap.get('id'));
  this.organizerService.getMovieById(this.movieId).subscribe({
    next: (res) => {
      this.movie = res;
    },
    error: (err) => {
      console.error('Failed to load movie', err);
      alert('Could not load movie details');
    }
  });
}

  loadMovie() {
    this.organizerService.getMovieById(this.movieId).subscribe({
      next: (res) => {
        this.movie = res;
      },
      error: (err) => {
        console.error(err);
        this.message = 'Failed to load movie details';
      }
    });
  }

  updateMovie() {
  const payload = {
    ...this.movie,
    showDate: this.movie.showDate,  // keep YYYY-MM-DD
    showTime: this.movie.showTime + ":00"  // add seconds so backend parses as TimeSpan
  };

  console.log("Payload:", payload);

  this.organizerService.updateMovie(this.movieId, payload).subscribe({
    next: () => {
      alert('Movie updated successfully!');
      this.router.navigate(['/organizer/dashboard']);
    },
    error: (err) => {
      console.error(err);
      alert('Failed to update movie');
    }
  });
}

}
