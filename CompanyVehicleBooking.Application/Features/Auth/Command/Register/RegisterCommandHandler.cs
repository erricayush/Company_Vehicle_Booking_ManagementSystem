using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.Register
{
    public class RegisterCommandHandler(IAuthService authService) : IRequestHandler<RegisterCommand, RegisterCommandResponse>
    {
        private readonly IAuthService _authService = authService;

        public async Task<RegisterCommandResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RegisterAsync(request, cancellationToken);
        }
    }
}
