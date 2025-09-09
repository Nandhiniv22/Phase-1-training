import { Component, OnInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';

interface Seat {
  id: string; // "A1"
  row: string;
  number: number;
  status: 'available' | 'selected' | 'booked';
  price: number;
  type?: string;
}

@Component({
  selector: 'app-booking',
  templateUrl: './booking.component.html',
  styleUrls: ['./booking.component.css']
})
export class BookingComponent implements OnInit, OnDestroy {

  seats: Seat[] = [];
  rows: string[] = [];
  loading = true;
  errorMsg = '';

  private movieId!: number;
  private theatreId!: number;

  rawSeats: any[] = []; // for mapping seat numbers to SeatId

  // Payment
  paymentId: string | null = null;
  paymentUrl: SafeResourceUrl | null = null;
  paymentStatus: 'Pending' | 'Paid' | 'Cancelled' | null = null;
  pollInterval: any;

  constructor(
    private http: HttpClient,
    private route: ActivatedRoute,
    private sanitizer: DomSanitizer,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.movieId = Number(this.route.snapshot.paramMap.get('movieId'));
    this.theatreId = Number(this.route.snapshot.paramMap.get('theatreId'));
    this.fetchSeats(this.movieId, this.theatreId);
  }

  ngOnDestroy(): void {
    if (this.pollInterval) clearInterval(this.pollInterval);
  }

  private fetchSeats(movieId: number, theatreId: number) {
  this.loading = true;
  this.errorMsg = '';
  const url = `http://localhost:5227/api/user/theatre/${theatreId}/seats?movieId=${movieId}`;

  this.http.get<any[]>(url).subscribe({
    next: (data) => {
      if (!data || data.length === 0) {
        // New movie or seats not in DB → create all seats available
        this.processRawSeats(this.demoRawSeats());
      } else {
        // Existing movie → show booked/available from DB
        this.processRawSeats(data);
      }
      this.loading = false;
    },
    error: (err) => {
      console.error(err);
      this.errorMsg = 'Could not load seats, using demo data.';
      this.processRawSeats(this.demoRawSeats());
      this.loading = false;
    }
  });
}


  private processRawSeats(raw: any[]) {
  this.rawSeats = raw; // store raw data for ID mapping
  const seatMap = new Map<string, Seat>();

  raw.forEach(r => {
    const seatNumber = r.SeatNumber ?? r.seatNumber ?? '';
    if (!seatNumber) return;

    const row = seatNumber.charAt(0).toUpperCase();
    const number = Number(seatNumber.slice(1)) || 0;
    const price = Number(r.Price ?? r.price ?? 150);
    const type = r.SeatType ?? '';

    // If seat exists in DB, use its availability; otherwise default to available
    const isAvailable = (r.IsAvailable ?? r.isAvailable) !== undefined ? Boolean(r.IsAvailable ?? r.isAvailable) : true;

    if (!seatMap.has(seatNumber)) {
      seatMap.set(seatNumber, {
        id: seatNumber,
        row,
        number,
        status: isAvailable ? 'available' : 'booked',
        price,
        type
      });
    }
  });

  this.seats = Array.from(seatMap.values())
    .sort((a, b) => b.row.localeCompare(a.row) || b.number - a.number);

  this.rows = Array.from(new Set(this.seats.map(s => s.row)))
    .sort((a, b) => b.localeCompare(a));
}


  toggleSeat(seat: Seat) {
    if (seat.status === 'booked') return;
    seat.status = seat.status === 'selected' ? 'available' : 'selected';
  }

  getSeatsByRow(row: string): Seat[] {
    return this.seats.filter(s => s.row === row).sort((a, b) => a.number - b.number);
  }

  // Seat IDs for backend
  get selectedSeatIds(): number[] {
  return this.seats
    .filter(s => s.status === 'selected')
    .map(s => {
      const raw = this.rawSeats.find(r =>
        (r.SeatNumber?.toString() ?? r.seatNumber?.toString()) === s.id
      );
      return raw ? Number(raw.SeatId ?? raw.seatId) : 0;
    })
    .filter(id => id > 0);
}

  // Labels for UI display
  get selectedSeatLabels(): string {
    const labels = this.seats.filter(s => s.status === 'selected').map(s => s.id);
    return labels.length ? labels.join(', ') : 'None';
  }

  // Total price
  get total(): number {
    return this.seats.filter(s => s.status === 'selected').reduce((acc, s) => acc + s.price, 0);
  }

  // Disable confirm button
  isConfirmDisabled(): boolean {
  return this.selectedSeatIds.length === 0 || (this.paymentStatus === 'Pending' && !!this.paymentId);
}

  // Confirm Booking & Initiate Payment
  confirmBooking() {
    if (!this.selectedSeatIds.length) return alert('Please select seats');

    const payload = {
      movieId: this.movieId,
      seatIds: this.selectedSeatIds
    };

    this.http.post<any>(`http://localhost:5227/api/booking/create`, payload, {
      headers: { Authorization: 'Bearer ' + localStorage.getItem('token') }
    }).subscribe({
      next: (res) => {
        alert('Seats booked successfully!. Redirecting to payment...');
        this.router.navigate(['/user/payment', res.bookingId, this.total]);
        this.seats.forEach(s => {
          if (s.status === 'selected') s.status = 'booked';
        });
        this.initiatePayment(res.bookingId, this.total);
      },
      error: (err) => {
        console.error('Booking failed', err);
        alert('Create booking failed: ' + err.error);
      }
    });
  }

  initiatePayment(bookingId: number, amount: number) {
    this.http.post<any>(`http://localhost:5227/api/payment/initiate`, { bookingId, amount }).subscribe({
      next: (res) => {
        this.paymentId = res.paymentId;
        this.paymentUrl = this.sanitizer.bypassSecurityTrustResourceUrl(res.paymentUrl);
        this.paymentStatus = 'Pending';
        this.pollInterval = setInterval(() => this.checkPaymentStatus(), 3000);
      },
      error: (err) => console.error(err)
    });
  }

  checkPaymentStatus() {
  if (!this.paymentId) return;
  this.http.get<any>(`http://localhost:5227/api/payment/status/${this.paymentId}`).subscribe({
    next: (res) => {
      this.paymentStatus = res.status;

      if (res.status === 'Paid') {
        if (this.pollInterval) clearInterval(this.pollInterval);

        // Redirect to booking-success page with paymentId
        this.router.navigate(['/user/booking-success', this.paymentId]);
      }

      // Optionally handle Cancelled here
    },
    error: (err) => console.error(err)
  });
}
  // Demo fallback
  private demoRawSeats(): any[] {
  const rows = ['A','B','C','D','E','F','G','H','I','J','K'];
  const out: any[] = [];

  rows.slice().reverse().forEach((row, rIdx) => {
    let type: string;
    let price: number;
    let seatsPerRow: number;

    if (row === 'A' || row === 'B') {
      type = 'Premium';
      price = 400;
      seatsPerRow = 10;
    } else if (['C','D','E','F'].includes(row)) {
      type = 'Gold';
      price = 250;
      seatsPerRow = 20;
    } else {
      type = 'Regular';
      price = 150;
      seatsPerRow = 20;
    }

    for (let n = seatsPerRow; n >= 1; n--) {
      const seatNum = `${row}${n}`;
      out.push({
        SeatNumber: seatNum,
        SeatType: type,
        Price: price,
        IsAvailable: true, // fully available for new movie
        SeatId: rIdx * 20 + n
      });
    }
  });

  return out;
}

}
