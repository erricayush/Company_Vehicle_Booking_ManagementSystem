namespace CompanyVehicleBooking.Application.Features.Auth.Command.Login
{
    public class LoginCommandResponse
    {
        public required string AccessToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
