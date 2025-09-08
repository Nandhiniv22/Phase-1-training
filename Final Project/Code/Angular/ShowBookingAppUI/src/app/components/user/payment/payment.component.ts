import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';

interface PaymentDetails {
  paymentId: string;
  bookingId: number;
  movieTitle: string;
  theatreName: string;
  seats: string[];
  bookingTime: string;
  amount: number;
  paymentStatus: 0 | 1 | 2; // 0 = Pending, 1 = Paid, 2 = Cancelled
}

@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrls: ['./payment.component.css']
})
export class PaymentComponent implements OnInit, OnDestroy {
  bookingId!: number;
  amount!: number;

  loading = true;
  paymentDetails: PaymentDetails | null = null;
  pollInterval: any;

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router) {}

  ngOnInit(): void {
    this.bookingId = Number(this.route.snapshot.paramMap.get('bookingId'));
    this.amount = Number(this.route.snapshot.paramMap.get('amount'));
    this.fetchPaymentDetails();
  }

  fetchPaymentDetails() {
    this.loading = true;
    this.http.get<PaymentDetails>(`http://localhost:5227/api/payment/details/${this.bookingId}`)
      .subscribe({
        next: res => {
          this.paymentDetails = res;
          this.loading = false;

          if (this.paymentDetails.paymentStatus === 0) {
            this.pollInterval = setInterval(() => this.checkPaymentStatus(), 3000);
          }
        },
        error: err => {
          console.error('Failed to load payment details', err);
          this.loading = false;
        }
      });
  }

  getPaymentStatusText(status: number): string {
    switch(status) {
      case 0: return 'Pending';
      case 1: return 'Paid';
      case 2: return 'Cancelled';
      default: return 'Unknown';
    }
  }

  checkPaymentStatus() {
    if (!this.paymentDetails) return;

    this.http.get<any>(`http://localhost:5227/api/payment/status/${this.paymentDetails.paymentId}`)
      .subscribe({
        next: res => {
          if (this.paymentDetails) this.paymentDetails.paymentStatus = res.status;
          if (res.status === 1) { // Paid
            clearInterval(this.pollInterval);
            this.router.navigate(['/user/booking-success', this.paymentDetails?.paymentId]);
          }
        },
        error: err => console.error(err)
      });
  }

  initiatePayment() {
    if (!this.paymentDetails) return;

    this.loading = true;
    this.http.post<any>(`http://localhost:5227/api/payment/initiate`, {
      bookingId: this.paymentDetails.bookingId,
      amount: this.paymentDetails.amount
    }).subscribe({
      next: res => {
        if (this.paymentDetails) {
          this.paymentDetails.paymentId = res.paymentId;
          this.paymentDetails.paymentStatus = 0; // Pending
        }
        this.loading = false;
        this.pollInterval = setInterval(() => this.checkPaymentStatus(), 3000);
      },
      error: err => {
        console.error(err);
        this.loading = false;
      }
    });
  }

  updatePaymentStatus(newStatus: 'Paid' | 'Cancelled') {
  if (!this.paymentDetails) return;

  const statusMap: Record<'Paid' | 'Cancelled', 0 | 1 | 2> = {
    'Paid': 1,
    'Cancelled': 2
  };
  const statusValue = statusMap[newStatus];

  this.http.post<any>(`http://localhost:5227/api/payment/update-status`, {
    PaymentId: this.paymentDetails.paymentId,
    Status: statusValue
  }).subscribe({
    next: res => {
      if (this.paymentDetails) this.paymentDetails.paymentStatus = statusValue; // TS happy
      if (newStatus === 'Paid' && this.paymentDetails) {
        alert('Payment Successful!. Redirecting to invoice...')
        this.router.navigate(['/user/booking-success', this.paymentDetails.paymentId]);
      }
    },
    error: err => console.error(err)
  });
}

  ngOnDestroy(): void {
    if (this.pollInterval) clearInterval(this.pollInterval);
  }
}
