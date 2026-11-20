using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.RejectBooking
{
    public class RejectBookingCommandHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<RejectBookingCommand, RejectBookingCommandResponse>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<RejectBookingCommandResponse> Handle(RejectBookingCommand request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.RejectBookingAsync(request, cancellationToken);
        }
    }
}
