export interface BookingRequest {
  movieId: number;
  seatIds: number[];
}

export interface Booking {
  bookingId: number;
  userId: number;
  movieId: number;
  seats: any[];
  bookingDate: string;
}
