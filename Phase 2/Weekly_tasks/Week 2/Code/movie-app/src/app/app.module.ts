import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MovieListComponent } from './component/movie-list/movie-list.component';
import { MovieFormComponent } from './component/movie-form/movie-form.component';
import { MovieDetailsComponent } from './component/movie-details/movie-details.component';
import { FormsModule } from '@angular/forms';
import { HomeComponent } from './component/home/home.component';

@NgModule({
  declarations: [
    AppComponent,
    MovieListComponent,
    MovieFormComponent,
    MovieDetailsComponent,
    HomeComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
