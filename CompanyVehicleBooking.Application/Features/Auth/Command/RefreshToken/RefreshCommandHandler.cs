using CompanyVehicleBooking.Application.ServicesContracts;
using MediatR;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.RefreshToken
{
    public class RefreshCommandHandler(IAuthService authService) : IRequestHandler<RefreshCommand, RefreshCommandResponse>
    {
        private readonly IAuthService _authService = authService;

        public async Task<RefreshCommandResponse> Handle(RefreshCommand request, CancellationToken cancellationToken)
        {
            return await _authService.RefreshAsync(request, cancellationToken);
        }
    }
}
