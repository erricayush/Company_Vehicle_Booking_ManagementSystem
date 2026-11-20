using FluentValidation;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.Login
{
    public class LoginCommandValidator :AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is requierd")
                .EmailAddress()
                .WithMessage("Please enter a valid email");

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
