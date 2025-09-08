import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { OrganizerService } from '../../../services/organizer.service';

@Component({
  selector: 'app-theatre-bookings',
  templateUrl: './theatre-bookings.component.html',
  styleUrls: ['./theatre-bookings.component.css']
})
export class TheatreBookingsComponent implements OnInit {
  theatreId!: number;
  bookings: any[] = [];
  seats: any[] = [];
  bookedSeatNumbers: string[] = [];
  loading = true;
  message = '';

  groupedSeats: { row: string, seats: any[] }[] = [];

  constructor(
    private route: ActivatedRoute,
    private organizerService: OrganizerService
  ) {}

  ngOnInit(): void {
    this.theatreId = Number(this.route.snapshot.paramMap.get('id'));
    this.loadBookings();
    this.loadSeats();
  }

  loadBookings() {
    this.organizerService.getBookings(this.theatreId).subscribe({
      next: (res) => {
        this.bookings = res;
        this.bookedSeatNumbers = res.flatMap((b: any) => b.seatNumbers);
        this.loading = false;
        if (this.bookings.length === 0) {
          this.message = 'No bookings yet for this theatre.';
        }
      },
      error: () => {
        this.message = 'Failed to load bookings.';
        this.loading = false;
      }
    });
  }

  loadSeats() {
    this.organizerService.getSeats(this.theatreId).subscribe({
      next: (res) => {
        this.seats = res;

        // Group seats by row (e.g., A, B, C...)
        const grouped: any = {};
        res.forEach((seat: any) => {
          const row = seat.seatNumber.charAt(0); // "A1" -> "A"
          if (!grouped[row]) grouped[row] = [];
          grouped[row].push(seat);
        });

        this.groupedSeats = Object.keys(grouped).map(row => ({
          row,
          seats: grouped[row].sort((a: any, b: any) =>
            parseInt(a.seatNumber.slice(1)) - parseInt(b.seatNumber.slice(1))
          )
        }));
      },
      error: () => {
        this.message = 'Failed to load seats.';
      }
    });
  }

  isBooked(seatNumber: string): boolean {
    return this.bookedSeatNumbers.includes(seatNumber);
  }
}
