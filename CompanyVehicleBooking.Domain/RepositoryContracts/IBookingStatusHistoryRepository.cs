using CompanyVehicleBooking.Domain.Entities;

namespace CompanyVehicleBooking.Domain.RepositoryContracts
{
    public interface IBookingStatusHistoryRepository
    {
        Task AddAsync(BookingStatusHistory statusHistory, CancellationToken cancellationToken);
    }
}
