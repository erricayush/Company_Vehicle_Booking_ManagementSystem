using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.Logout
{
    public class LogoutCommandHandler(IAuthService authService) : IRequestHandler<LogoutCommand, LogoutCommandResponse>
    {
        private readonly IAuthService _authService = authService;

        public async Task<LogoutCommandResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            return await _authService.LogoutAsync(request, cancellationToken);
        }
    }
}
