using MediatR;

namespace CompanyVehicleBooking.Application.Features.Auth.Command.RefreshToken
{
    public class RefreshCommand :IRequest<RefreshCommandResponse>
    {
        public required string AccessToken { get; set; } 
        public required string RefreshToken { get; set; }
    }
}
