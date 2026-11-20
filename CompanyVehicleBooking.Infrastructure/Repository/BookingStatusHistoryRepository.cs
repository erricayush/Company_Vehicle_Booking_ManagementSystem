using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Domain.RepositoryContracts;
using CompanyVehicleBooking.Infrastructure.Context;

namespace CompanyVehicleBooking.Infrastructure.Repository
{
    public class BookingStatusHistoryRepository(AppDbContext context) : IBookingStatusHistoryRepository
    {
        private readonly AppDbContext _context = context;

        public async Task AddAsync(BookingStatusHistory statusHistory, CancellationToken cancellationToken)
        {
             await _context.AddAsync(statusHistory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
