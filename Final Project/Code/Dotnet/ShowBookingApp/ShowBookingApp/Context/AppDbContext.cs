using Microsoft.EntityFrameworkCore;
using ShowBookingApp.Models;
using System.Collections.Generic;
using System.ComponentModel;

namespace ShowBookingApp.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Theatre> Theatres { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User ↔ Booking
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(b => b.User)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Organizer ↔ Theatre
            modelBuilder.Entity<Theatre>()
                .HasOne(t => t.Organizer)
                .WithMany(u => u.Theatres)
                .HasForeignKey(t => t.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Theatre ↔ Movie
            modelBuilder.Entity<Movie>()
                .HasOne(m => m.Theatre)
                .WithMany(t => t.Movies)
                .HasForeignKey(m => m.TheatreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking ↔ Theatre
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Theatre)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TheatreId)
                .OnDelete(DeleteBehavior.Restrict); // <- changed from Cascade to Restrict

            // Seat ↔ Movie
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Movie)
                .WithMany(m => m.Seats)
                .HasForeignKey(s => s.MovieId)
                .OnDelete(DeleteBehavior.Restrict);

            // Theatre ↔ Seat
            modelBuilder.Entity<Seat>()
                .HasOne(s => s.Theatre)
                .WithMany(t => t.Seats)
                .HasForeignKey(s => s.TheatreId)
                .OnDelete(DeleteBehavior.Cascade);

            // Booking ↔ Seat (Many-to-Many)
            modelBuilder.Entity<Booking>()
                .HasMany(b => b.Seats)
                .WithMany(s => s.Bookings)
                .UsingEntity<Dictionary<string, object>>(
                    "BookingSeats",
                    j => j.HasOne<Seat>().WithMany().HasForeignKey("SeatId").OnDelete(DeleteBehavior.NoAction),
                    j => j.HasOne<Booking>().WithMany().HasForeignKey("BookingId").OnDelete(DeleteBehavior.Cascade)
                );

            modelBuilder.Entity<Movie>()
                .HasIndex(m => new { m.ShowDate, m.ShowTime });
        }


    }
}
