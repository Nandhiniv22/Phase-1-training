import { Injectable } from '@angular/core';
import { Movie } from 'src/models/movie.model';

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  constructor() { }
  private movies: Movie[] = [
    { id: 1, name: 'Alice', gender: 'Female', age: 25, dob: '2000-01-15' },
    { id: 2, name: 'Bob', gender: 'Male', age: 30, dob: '1995-06-10' }
  ];

  getMovies(filter?: string): Movie[] {
    if (filter === 'male') return this.movies.filter(m => m.gender === 'Male');
    if (filter === 'female') return this.movies.filter(m => m.gender === 'Female');
    return this.movies;
  }

  addMovie(movie: Movie) {
    movie.id = this.movies.length + 1;
    this.movies.push(movie);
  }

  getMovieById(id: number): Movie | undefined {
    return this.movies.find(m => m.id === id);
  }

  deleteMovie(id: number) {
    this.movies = this.movies.filter(m => m.id !== id);
  }
}
