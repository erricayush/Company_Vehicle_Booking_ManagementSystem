using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.ApproveBooking
{
    public class ApproveBookingCommandHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<ApproveBookingCommand, ApproveBookingCommandResponse>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<ApproveBookingCommandResponse> Handle(ApproveBookingCommand request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.ApproveBookingAsync(request, cancellationToken);
        }
    }
}
