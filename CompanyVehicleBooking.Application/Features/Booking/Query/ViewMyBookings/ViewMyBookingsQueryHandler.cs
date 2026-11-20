using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Query.ViewMyBookings
{
    public class ViewMyBookingsQueryHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<ViewMyBookingsQuery, List<ViewMyBookingsQueryResponse>>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<List<ViewMyBookingsQueryResponse>> Handle(ViewMyBookingsQuery request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.ViewMyBookingsAsync(request, cancellationToken);
        }
    }
}
