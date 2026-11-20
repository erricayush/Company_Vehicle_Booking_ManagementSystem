using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyVehicleBooking.Application.Features.Tracking.Command.CompleteTrip
{
    public class CompleteTripCommandValidator :AbstractValidator<CompleteTripCommand>
    {
        public CompleteTripCommandValidator()
        {
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("BookingId is required.")
                .NotNull().WithMessage("BookingId cannot be null.");
        }
    }
}
