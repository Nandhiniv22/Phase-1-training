import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface BookingDetails {
  paymentId: string;
  bookingId: number;
  movieTitle: string;
  theatreName: string;
  seats: string[];
  bookingTime: string;
  amount: number;
  paymentStatus: number; // 0 = Pending, 1 = Paid, 2 = Cancelled
}

@Component({
  selector: 'app-booking-success',
  templateUrl: './booking-success.component.html',
  styleUrls: ['./booking-success.component.css']
})
export class BookingSuccessComponent implements OnInit {
  paymentId!: string;
  bookingDetails: BookingDetails | null = null;
  loading = true;
  errorMessage: string = '';

  constructor(private route: ActivatedRoute, private http: HttpClient) {}

  ngOnInit(): void {
    this.paymentId = this.route.snapshot.paramMap.get('paymentId')!;
    this.fetchBookingDetails();
  }

  fetchBookingDetails() {
    this.http.get<BookingDetails>(`http://localhost:5227/api/payment/${this.paymentId}`)
      .subscribe({
        next: res => {
          this.bookingDetails = res;
          this.loading = false;
        },
        error: err => {
          console.error('Failed to load booking details', err);
          this.errorMessage = 'Could not load booking details.';
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
}
