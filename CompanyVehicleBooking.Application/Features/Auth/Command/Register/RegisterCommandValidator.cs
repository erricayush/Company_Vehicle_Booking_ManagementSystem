using FluentValidation;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.Register
{
    public class RegisterCommandValidator :AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Please enter a valid email");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
