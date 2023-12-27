using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
namespace TestGp.Models

{
    public class Mydb : DbContext
    {

        public Mydb(DbContextOptions<Mydb> options) : base(options) { }

       
        public DbSet<Register> Registers{ get; set; }
        public DbSet<Login> Logins { get; set; }
       public DbSet<Booking>Bookings { get; set; }


    }


}
