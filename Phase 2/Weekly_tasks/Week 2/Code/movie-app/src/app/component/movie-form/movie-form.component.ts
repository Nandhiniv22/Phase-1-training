import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MovieService } from 'src/app/services/movie.service';
import { Movie } from 'src/models/movie.model';

@Component({
  selector: 'app-movie-form',
  templateUrl: './movie-form.component.html',
  styleUrls: ['./movie-form.component.scss']
})
export class MovieFormComponent {
  movie: Movie = { id: 0, name: '', gender: '', age: 0, dob: '' };

  constructor(private movieService: MovieService, private router: Router) {}

  onSubmit() {
    this.movieService.addMovie(this.movie);
    this.router.navigate(['/list']);
  }
}
