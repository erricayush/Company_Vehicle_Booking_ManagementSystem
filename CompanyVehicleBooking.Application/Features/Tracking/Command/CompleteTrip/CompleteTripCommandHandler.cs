using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Tracking.Command.CompleteTrip
{
    public class CompleteTripCommandHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<CompleteTripCommand, CompleteTripCommandResponse>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<CompleteTripCommandResponse> Handle(CompleteTripCommand request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.CompleteTripAsync(request, cancellationToken);
        }
    }
}
