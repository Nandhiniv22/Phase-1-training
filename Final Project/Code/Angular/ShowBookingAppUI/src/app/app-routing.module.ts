import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

// Public
import { LandingComponent } from './components/landing/landing/landing.component';
import { LoginComponent } from './components/auth/login/login.component';
import { RegisterComponent } from './components/auth/register/register.component';

// Dashboards
import { AdminDashboardComponent } from './components/admin/admin-dashboard/admin-dashboard.component';
import { OrganizerDashboardComponent } from './components/organizer/organizer-dashboard/organizer-dashboard.component';
import { UserDashboardComponent } from './components/user/user-dashboard/user-dashboard.component';

// Organizer child pages
import { AddTheatreComponent } from './components/organizer/add-theatre/add-theatre.component';
import { AddMovieComponent } from './components/organizer/add-movie/add-movie.component';
import { TheatreBookingsComponent } from './components/organizer/theatre-bookings/theatre-bookings.component';

// Guards
import { AuthGuard } from './guards/auth.guard';
import { RoleGuard } from './guards/role.guard';
import { ProfileComponent } from './components/profile/profile.component';
import { BookingComponent } from './components/user/booking/booking.component';
import { UsersComponent } from './components/admin/users/users.component';
import { OrganizersComponent } from './components/admin/organizers/organizers.component';
import { StatisticsComponent } from './components/admin/statistics/statistics.component';
import { EditMovieComponent } from './components/organizer/edit-movie/edit-movie.component';
import { PaymentComponent } from './components/user/payment/payment.component';
import { BookingSuccessComponent } from './components/user/booking-success/booking-success.component';
import { BookingHistoryComponent } from './components/user/booking-history/booking-history.component';

const routes: Routes = [
  { path: '', component: LandingComponent },

  // auth
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },

  // admin routes (protected)
  {
    path: 'admin',
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Admin' },
    children: [
      { path: 'dashboard', component: AdminDashboardComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'users', component: UsersComponent },
      { path: 'organizers', component: OrganizersComponent },
      { path: 'reports', component: StatisticsComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // organizer routes (protected)
  {
    path: 'organizer',
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'Organizer' },
    children: [
      { path: 'dashboard', component: OrganizerDashboardComponent },
      { path: 'add-theatre', component: AddTheatreComponent },
      { path: 'theatre/:id/add-movie', component: AddMovieComponent },
      { path: 'theatre/:theatreId/edit-movie/:movieId', component: EditMovieComponent },
      { path: 'theatre/:id/bookings', component: TheatreBookingsComponent },
      { path: 'edit-movie/:id', component: EditMovieComponent },
      { path: 'profile', component: ProfileComponent },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
    ]
  },

  // user routes (protected)
  {
    path: 'user',
    canActivate: [AuthGuard, RoleGuard],
    data: { role: 'User' },
    children: [
      { path: 'home', component: UserDashboardComponent },
      { path: 'profile', component: ProfileComponent },
      { path: 'booking/:movieId/:theatreId', component: BookingComponent },
      { path: 'booking-success/:paymentId', component: BookingSuccessComponent },
      { path: 'payment/:bookingId/:amount', component: PaymentComponent },
      { path: 'booking-history', component: BookingHistoryComponent },
      { path: '', redirectTo: 'home', pathMatch: 'full' },
    ]
  },

  // fallback
  { path: '**', redirectTo: '/login' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}
