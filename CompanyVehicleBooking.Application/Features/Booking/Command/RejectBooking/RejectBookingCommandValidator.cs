using FluentValidation;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.RejectBooking
{
    public class RejectBookingCommandValidator :AbstractValidator<RejectBookingCommand>
    {
        public RejectBookingCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty().WithMessage("Booking id is required");
        }
    }
}
