using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.ApproveBooking
{
    public class ApproveBookingCommand :IRequest<ApproveBookingCommandResponse>
    {
        public Guid BookingId {  get; set; }
    }
}
