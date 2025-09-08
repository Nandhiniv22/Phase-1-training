import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LoginComponent } from './components/auth/login/login.component';
import { JwtInterceptor } from './jwt.interceptor';
import { RegisterComponent } from './components/auth/register/register.component';
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { OrganizerDashboardComponent } from './components/organizer/organizer-dashboard/organizer-dashboard.component';
import { UserDashboardComponent } from './components/user/user-dashboard/user-dashboard.component';
import { LandingComponent } from './components/landing/landing/landing.component';
import { AddTheatreComponent } from './components/organizer/add-theatre/add-theatre.component';
import { AddMovieComponent } from './components/organizer/add-movie/add-movie.component';
import { TheatreBookingsComponent } from './components/organizer/theatre-bookings/theatre-bookings.component';
import { OrganizerNavbarComponent } from './components/organizer/organizer-navbar/organizer-navbar.component';
import { UserNavbarComponent } from './components/user/user-navbar/user-navbar.component';
import { AdminNavbarComponent } from './components/admin/admin-navbar/admin-navbar.component';
import { LandingNavbarComponent } from './components/landing/landing-navbar/landing-navbar.component';
import { RouterModule } from '@angular/router';
import { ProfileComponent } from './components/profile/profile.component';
import { BookingComponent } from './components/user/booking/booking.component';
import { UsersComponent } from './components/admin/users/users.component';
import { OrganizersComponent } from './components/admin/organizers/organizers.component';
import { StatisticsComponent } from './components/admin/statistics/statistics.component';
import { NgChartsModule } from 'ng2-charts';
import { EditMovieComponent } from './components/organizer/edit-movie/edit-movie.component';
import { SafePipe } from './safe.pipe';
import { PaymentComponent } from './components/user/payment/payment.component';
import { BookingSuccessComponent } from './components/user/booking-success/booking-success.component';
import { BookingHistoryComponent } from './components/user/booking-history/booking-history.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    AdminDashboardComponent,
    OrganizerDashboardComponent,
    UserDashboardComponent,
    LandingComponent,
    AddTheatreComponent,
    AddMovieComponent,
    TheatreBookingsComponent,
    OrganizerNavbarComponent,
    UserNavbarComponent,
    AdminNavbarComponent,
    LandingNavbarComponent,
    ProfileComponent,
    BookingComponent,
    UsersComponent,
    OrganizersComponent,
    StatisticsComponent,
    EditMovieComponent,
    SafePipe,
    PaymentComponent,
    BookingSuccessComponent,
    BookingHistoryComponent
  ],
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    AppRoutingModule,
    RouterModule,
    NgChartsModule,
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
