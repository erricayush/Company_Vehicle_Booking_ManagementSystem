using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Tracking.Command
{
    public class AllocateVehicleCommandHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<AllocateVehicleCommand, AllocateVehicleCommandResponse>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<AllocateVehicleCommandResponse> Handle(AllocateVehicleCommand request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.AllocateVehicleAsync(request, cancellationToken);
        }
    }
}
