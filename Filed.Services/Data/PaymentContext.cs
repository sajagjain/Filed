using Filed.Services.Data.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Filed.Services.Data
{
    public class PaymentContext : DbContext
    {
        public PaymentContext()
        {

        }

        public PaymentContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        public DbSet<Payment> Payments { get; set; }
        public DbSet<PaymentState> PaymentStates { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Payment>()
                .HasMany(a => a.PaymentState)
                .WithOne(a => a.Payment)
                .HasForeignKey(s => s.PaymentId);
        }
    }
}
