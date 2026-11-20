using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Domain.Enums;
using CompanyVehicleBooking.Domain.RepositoryContracts;
using CompanyVehicleBooking.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanyVehicleBooking.Infrastructure.Repository
{
    public class VehicleBookingRepository(AppDbContext context) : IVehicleBookingRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<VehicleBooking?> GetBookingByBookingIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.FirstOrDefaultAsync(vb => vb.Id == bookingId, cancellationToken);
        }


        public async Task<VehicleBooking?> GetBookingByIdAsync(Guid bookingId, CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.FirstOrDefaultAsync(vb => vb.Id == bookingId, cancellationToken);
        }

        public async Task<VehicleBooking?> GetBookingByVehicleIdAsync(Guid vehicleId, CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.FirstOrDefaultAsync(vb => vb.VehicleId == vehicleId, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<VehicleBooking>> GetAllBookingAsync(int pageNumber, int pageSize, string? sortOrder, string? sortBy, string? search, CancellationToken cancellationToken)
        {
            IQueryable<VehicleBooking> query = _context.VehicleBookings.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(v => v.Destination.Contains(search)
                || v.Purpose.Contains(search)

                );
            }

            query = sortBy?.ToLower() switch
            {
                "purpose" => sortOrder?.ToLower() == "desc"
                   ? query.OrderByDescending(v => v.Purpose)
                   : query.OrderBy(v => v.Purpose),

                "destination" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.Destination)
                  : query.OrderBy(v => v.Destination),

                "returndate" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.ReturnDate)
                  : query.OrderBy(v => v.ReturnDate),

                "createddate" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.CreatedDate)
                  : query.OrderBy(v => v.CreatedDate),

                "traveldate" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.TravelDate)
                  : query.OrderBy(v => v.TravelDate),

                _ => query.OrderByDescending(v => v.CreatedDate)
            };

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : Math.Min(pageSize, 100);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync(cancellationToken);

        }

        public async Task<bool> VehicleHasAlreadyApprovedBookingAsync(Guid vehicleId,DateTime returnDate, DateTime travelDate, Guid id, CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.AnyAsync(vb =>
                   vb.VehicleId == vehicleId 
                && vb.BookingStatus == BookingStatus.Approved 
                && vb.Id != id 
                && vb.TravelDate <= returnDate
                && vb.ReturnDate >= travelDate,
                cancellationToken);
        }

        public async Task<List<VehicleBooking>> GetBookingByEmployeeIdAsync(Guid employeeId, string? search, string? sortOrder, string? sortBy, int pageSize, int pageNumber, CancellationToken cancellationToken)
        {
            IQueryable<VehicleBooking> query = _context.VehicleBookings.AsNoTracking().Where(vb => vb.EmployeeId == employeeId);
            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();
                query = query.Where(v => v.Destination.Contains(search)
                || v.Purpose.Contains(search)

                );
            }

            query = sortBy?.ToLower() switch
            {
                "purpose" => sortOrder?.ToLower() == "desc"
                   ? query.OrderByDescending(v => v.Purpose)
                   : query.OrderBy(v => v.Purpose),

                "destination" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.Destination)
                  : query.OrderBy(v => v.Destination),

                "returndate" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.ReturnDate)
                  : query.OrderBy(v => v.ReturnDate),

                "createddate" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.CreatedDate)
                  : query.OrderBy(v => v.CreatedDate),

                "traveldate" => sortOrder?.ToLower() == "desc"
                  ? query.OrderByDescending(v => v.TravelDate)
                  : query.OrderBy(v => v.TravelDate),

                _ => query.OrderByDescending(v => v.CreatedDate)
            };

            pageNumber = pageNumber < 1 ? 1 : pageNumber;
            pageSize = pageSize < 1 ? 1 : Math.Min(pageSize, 100);

            query = query.Skip((pageNumber - 1) * pageSize).Take(pageSize);
            return await query.ToListAsync(cancellationToken);
        }


        public async Task AddAsync(VehicleBooking booking, CancellationToken cancellationToken)
        {
            await _context.VehicleBookings.AddAsync(booking, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(VehicleBooking booking, CancellationToken cancellationToken)
        {
            _context.VehicleBookings.Update(booking);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> EmployeeHasAlreadyApprovedBookingAsync(Guid employeeId,DateTime returnDate,DateTime travelDate,Guid id,
    CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.AnyAsync(vb =>
                vb.EmployeeId == employeeId
                && vb.BookingStatus == BookingStatus.Approved
                 && vb.Id != id 
                 && vb.TravelDate <= returnDate
                 && vb.ReturnDate >= travelDate,
                cancellationToken);
        }



        public async Task<bool> VehicleHasAlreadyOverlapBookingAsync(Guid vehicleId,DateTime returnDate,DateTime travelDate,CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.AnyAsync(vb =>
                vb.VehicleId == vehicleId 
                &&vb.BookingStatus == BookingStatus.Approved 
                &&vb.TravelDate <= returnDate
                && vb.ReturnDate >= travelDate,
                cancellationToken);
        }

        public async Task<bool> EmployeeHasAlreadyOverlapBookingAsync(Guid employeeId,DateTime returnDate,DateTime travelDate,CancellationToken cancellationToken)
        {
            return await _context.VehicleBookings.AnyAsync(
                vb => vb.EmployeeId == employeeId
                  && vb.BookingStatus == BookingStatus.Approved
                  && vb.TravelDate <= returnDate
                  && vb.ReturnDate >= travelDate,
                cancellationToken);
        }
    }
}
