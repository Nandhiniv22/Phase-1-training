export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  contactNumber: string;
}

export interface RegisterResponse {
  message: string;
  userId: number;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  token: string;
  expiresAt: Date;
  userId: number;
  name: string;
  role: string;
  isApprovedOrganizer: boolean;
}

export interface User {
  userId: number;
  name: string;
  email: string;
  role: string;
  isApprovedOrganizer: boolean;
  isBlocked: boolean;
}

// src/app/models/user.model.ts

export interface Seat {
  seatId: number;
  seatNumber: string;
  seatType: string;
  price: number;
  isAvailable: boolean;
}

export interface Booking {
  bookingId: number;
  movieId: number;
  movieTitle: string;
  seats: Seat[];
  totalPrice: number;
  seatNumbers?: string; // computed in component
}

