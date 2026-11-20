using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace CompanyVehicleBooking.Application.Features.Tracking.Command
{
    public class AllocateVehicleCommandValidator : AbstractValidator<AllocateVehicleCommand>
    {
        public AllocateVehicleCommandValidator()
        {
            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("VehicleId is required.")
                .NotNull().WithMessage("VehicleId cannot be null.");
            RuleFor(x => x.BookingId)
                .NotEmpty().WithMessage("BookingId is required.")
                .NotNull().WithMessage("BookingId cannot be null.");
        }
    }
}
