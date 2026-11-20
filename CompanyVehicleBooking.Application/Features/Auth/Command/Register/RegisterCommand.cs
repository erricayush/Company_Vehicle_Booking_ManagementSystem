using MediatR;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.Register
{
    public class RegisterCommand  : IRequest<RegisterCommandResponse>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
