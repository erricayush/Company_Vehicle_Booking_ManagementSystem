using CompanyVehicleBooking.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CompanyVehicleBooking.Infrastructure.Context
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<ApplicationUser,IdentityRole<Guid>,Guid>(options)
    {
        public DbSet<BookingStatusHistory> BookingStatusHistories { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleBooking> VehicleBookings { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<VehicleBooking>()
                .HasOne(vb => vb.Vehicle)
                .WithMany(v => v.VehicleBookings)
                .HasForeignKey(vb => vb.VehicleId);

            builder.Entity<VehicleBooking>()
                .HasOne(vb => vb.Employee)
                .WithMany(e => e.VehicleBookings)
                .HasForeignKey(vb => vb.EmployeeId);

            builder.Entity<BookingStatusHistory>()
                .HasOne(bs=>bs.VehicleBooking)
                .WithMany(vb=>vb.BookingStatusHistories)
                .HasForeignKey(bs=>bs.VehicleBookingId);
        }
    }
}
