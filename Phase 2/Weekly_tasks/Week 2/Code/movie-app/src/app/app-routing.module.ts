import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MovieListComponent } from './component/movie-list/movie-list.component';
import { MovieDetailsComponent } from './component/movie-details/movie-details.component';
import { MovieFormComponent } from './component/movie-form/movie-form.component';
import { HomeComponent } from './component/home/home.component';

const routes: Routes = [
  { path: '', component: HomeComponent },     
  { path: 'list', component: MovieListComponent },
  { path: 'form', component: MovieFormComponent },
  { path: 'details/:id', component: MovieDetailsComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
