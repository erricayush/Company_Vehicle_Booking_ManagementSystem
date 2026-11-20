namespace CompanyVehicleBooking.Application.Features.Auth.Command.RefreshToken
{
    public class RefreshCommandResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
