using FluentValidation;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.ApproveBooking
{
    public class ApproveBookingCommandValidator :AbstractValidator<ApproveBookingCommand>
    {
        public ApproveBookingCommandValidator()
        {
            RuleFor(x => x.BookingId).NotEmpty().WithMessage("Booking id is required");
        }
    }
}
