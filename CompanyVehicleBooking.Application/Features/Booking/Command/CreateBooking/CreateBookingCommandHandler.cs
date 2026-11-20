using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.CreateBooking
{
    public class CreateBookingCommandHandler(IVehicleBookingService vehicleBooking) : IRequestHandler<CreateBookingCommand, CreateBookingCommandResponse>
    {
        private readonly IVehicleBookingService _vehicleBooking = vehicleBooking;

        public async Task<CreateBookingCommandResponse> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            return await _vehicleBooking.CreateBookingAsync(request, cancellationToken);
        }
    }
}
