using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Query.GetBookingById
{
    public class GetBookingByIdQueryHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<GetBookingByIdQuery, GetBookingByIdQueryResponse>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<GetBookingByIdQueryResponse> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.GetBookingByIdAsync(request, cancellationToken);
        }
    }
}
