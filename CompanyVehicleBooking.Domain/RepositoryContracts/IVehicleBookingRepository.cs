using CompanyVehicleBooking.Domain.Entities;

namespace CompanyVehicleBooking.Domain.RepositoryContracts
{
    public interface IVehicleBookingRepository
    {
        Task AddAsync(VehicleBooking booking, CancellationToken cancellationToken);
        Task<bool> EmployeeHasAlreadyApprovedBookingAsync(Guid employeeId, DateTime returnDate, DateTime travelDate, Guid id, CancellationToken cancellationToken);
        Task<bool> EmployeeHasAlreadyOverlapBookingAsync(Guid employeeId, DateTime returnDate, DateTime travelDate, CancellationToken cancellationToken);
        Task<List<VehicleBooking>> GetAllBookingAsync(int pageNumber, int pageSize, string? sortOrder, string? sortBy, string? search, CancellationToken cancellationToken);
        Task<VehicleBooking?> GetBookingByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken);
        Task<List<VehicleBooking>> GetBookingByEmployeeIdAsync(Guid employeeId, string? search, string? sortOrder, string? sortBy, int pageSize, int pageNumber, CancellationToken cancellationToken);

        Task<VehicleBooking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken);
        Task<VehicleBooking?> GetBookingByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken);
        Task SaveChangesAsync(CancellationToken cancellationToken);
        Task UpdateAsync(VehicleBooking booking, CancellationToken cancellationToken);
        Task<bool> VehicleHasAlreadyApprovedBookingAsync(Guid vehicleId, DateTime returnDate, DateTime travelDate, Guid id, CancellationToken cancellationToken);
        Task<bool> VehicleHasAlreadyOverlapBookingAsync(Guid vehicleId, DateTime returnDate, DateTime travelDate, CancellationToken cancellationToken);
     

    }
}
