using CompanyVehicleBooking.Application.Features.Auth.Command.Login;
using CompanyVehicleBooking.Application.Features.Auth.Command.Logout;
using CompanyVehicleBooking.Application.Features.Auth.Command.RefreshToken;
using CompanyVehicleBooking.Application.Features.Auth.Command.Register;

namespace CompanyVehicleBooking.Application.ServicesContracts
{
    public interface IAuthService
    {
        Task<LoginCommandResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken);
        Task<LogoutCommandResponse> LogoutAsync(LogoutCommand request, CancellationToken cancellationToken);
        Task<RefreshCommandResponse> RefreshAsync(RefreshCommand request, CancellationToken cancellationToken);
        Task<RegisterCommandResponse> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken);
    }
}
