using FluentValidation;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.CreateBooking
{
    public class CreateBookingCommandValidator :AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingCommandValidator()
        {
            RuleFor(x => x.PurposeOfTravel)
                .NotEmpty()
                .WithMessage("Purpose of travel is required");

            RuleFor(x => x.TravelDate).NotEmpty()
                 .WithMessage("travel date is required");

            RuleFor(x => x.ReturnDate).NotEmpty()
                .WithMessage("Return date is required");

            RuleFor(x=>x.Destination).NotEmpty()
                .WithMessage("Destination is required");

            RuleFor(x => x.VehicleId).NotEmpty()
                .NotNull().WithMessage("Vehicle id should not be null")
                .WithMessage("VehicleId is required");

        }
    }
}
