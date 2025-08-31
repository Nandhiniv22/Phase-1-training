import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { MovieService } from 'src/app/services/movie.service';
import { Movie } from 'src/models/movie.model';

@Component({
  selector: 'app-movie-list',
  templateUrl: './movie-list.component.html',
  styleUrls: ['./movie-list.component.scss']
})
export class MovieListComponent {
  movies: Movie[] = [];
  maleCount = 0;
  femaleCount = 0;
  averageAge = 0;

  constructor(private movieService: MovieService, private router: Router) {}

  ngOnInit(): void {
    this.loadMovies();
  }

  loadMovies(filter: string = '') {
    this.movies = this.movieService.getMovies(filter);
    this.maleCount = this.movieService.getMovies('male').length;
    this.femaleCount = this.movieService.getMovies('female').length;
    this.averageAge = this.movies.length
      ? parseFloat((this.movies.reduce((sum, m) => sum + m.age, 0) / this.movies.length).toFixed(2))
      : 0;
  }

  filterMovies(filter: string) {
    this.loadMovies(filter);
  }

  viewDetails(id: number) {
    this.router.navigate(['/details', id]);
  }

  deleteMovie(id: number) {
    this.movieService.deleteMovie(id);
    this.loadMovies();
  }
}
