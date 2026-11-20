using CompanyVehicleBooking.Domain.Entities;

namespace CompanyVehicleBooking.Domain.RepositoryContracts
{
    public interface IVehicleRepository
    {
        Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken);
    }
}
