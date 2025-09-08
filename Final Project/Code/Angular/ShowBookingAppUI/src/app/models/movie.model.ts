
export interface CreateMovie {
  title: string;
  language: string;
  description: string;
  durationMinutes: number;
}

export interface TheatreDto {
  theatreId: number;
  name: string;
  location: string;
}

export interface Movie {
  movieId: number;
  title: string;
  screenType: string;
  seatCategories: string[];
  bookings: number;
  theatre: TheatreDto;
  showDate: string;   // "2025-09-25"
  showTime: string;   // "18:30:00"
  seats?: Seat[];
}

export interface Seat {
  seatId: number;
  seatNumber: string;
  seatType: string;
  price: number;
  isAvailable: boolean;
}
