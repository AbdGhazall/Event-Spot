using Microsoft.EntityFrameworkCore;

namespace TestGp.Models

{
    public class Mydb : DbContext
    {
        public Mydb(DbContextOptions<Mydb> options) : base(options)
        {
        }

        public DbSet<Register> Registers { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Event> Events { get; set; }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<OrganizerRegister> organizerRegisters { get; set; }
    }
}