using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.RejectBooking
{
    public class RejectBookingCommand :IRequest<RejectBookingCommandResponse>
    {
        public Guid BookingId { get; set; }
    }
}
