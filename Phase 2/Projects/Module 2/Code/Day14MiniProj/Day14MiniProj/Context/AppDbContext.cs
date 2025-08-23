using Day14MiniProj.Models;
using Microsoft.EntityFrameworkCore;

namespace Day14MiniProj.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Bank> BankStore { get; set; }
    }
}
