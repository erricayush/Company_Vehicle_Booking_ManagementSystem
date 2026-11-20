using MediatR;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.Login
{
    public class LoginCommand :IRequest<LoginCommandResponse>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
