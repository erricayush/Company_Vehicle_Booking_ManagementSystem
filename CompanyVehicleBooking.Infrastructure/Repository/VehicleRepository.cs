using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Domain.RepositoryContracts;
using CompanyVehicleBooking.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanyVehicleBooking.Infrastructure.Repository
{
    public class VehicleRepository(AppDbContext context) : IVehicleRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<Vehicle?> GetByIdAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            return await _context.Vehicles.FirstOrDefaultAsync(v => v.Id == vehicleId, cancellationToken);
        }
    }
}
  