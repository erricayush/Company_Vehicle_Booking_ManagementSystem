using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Query.GetAllBooking
{
    public class GetAllBookingQueryHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<GetAllBookingQuery, List<GetAllBookingQueryResponse>>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<List<GetAllBookingQueryResponse>> Handle(GetAllBookingQuery request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.GetAllBookingAsync(request, cancellationToken);
        }
    }
}
