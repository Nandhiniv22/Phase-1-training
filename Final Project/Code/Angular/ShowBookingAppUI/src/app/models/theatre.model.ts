export interface Theatre {
  theatreId: number;
  name: string;
  location: string;
  organizerId: number;
}

export interface CreateTheatre {
  name: string;
  location: string;
}
