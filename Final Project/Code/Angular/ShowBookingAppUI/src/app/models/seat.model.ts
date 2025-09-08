export interface Seat {
  seatId: number;
  seatNumber: string;
  seatType: string;
  price: number;
  isAvailable: boolean;
  theatreId: number;
}

export interface CreateSeat {
  seatNumber: string;
  seatType: string;
  price: number;
}
