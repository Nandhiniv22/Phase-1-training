import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AuthService } from 'src/app/services/auth.service'; // your auth service

interface Booking {
  bookingId: number;
  movieTitle: string;
  theatreName: string;
  seats: string[];
  bookingTime: string;
  paymentId: string;
  amount: number;
  paymentStatus: number;
}

@Component({
  selector: 'app-booking-history',
  templateUrl: './booking-history.component.html',
  styleUrls: ['./booking-history.component.css']
})
export class BookingHistoryComponent implements OnInit {
  userId!: number; // will get from AuthService
  bookings: Booking[] = [];
  loading = true;
  errorMessage = '';

  constructor(private http: HttpClient, private authService: AuthService) {}

  ngOnInit(): void {
    this.userId = this.authService.getLoggedInUserId(); // get logged-in user
    this.fetchBookingHistory();
  }

  fetchBookingHistory() {
    this.loading = true;
    this.http.get<Booking[]>(`http://localhost:5227/api/payment/user/${this.userId}`)
      .subscribe({
        next: (res) => {
          this.bookings = res || [];
          this.loading = false;
        },
        error: (err) => {
          console.error(err);
          this.errorMessage = 'Could not load booking history.';
          this.loading = false;
        }
      });
  }

  getPaymentStatusText(status: number): string {
    switch (status) {
      case 0: return 'Pending';
      case 1: return 'Paid';
      case 2: return 'Cancelled';
      default: return 'Unknown';
    }
  }

  formatSeats(seats: string[] | null | undefined): string {
    return seats?.join(', ') ?? 'N/A';
  }

  formatBookingTime(bookingTime: string | null | undefined): string {
    return bookingTime ? new Date(bookingTime).toLocaleString() : 'N/A';
  }
}
