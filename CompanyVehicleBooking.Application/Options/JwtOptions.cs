namespace CompanyVehicleBooking.Application.Options
{
    public class JwtOptions 
    {
        public required string ValidAudience { get; set; }
        public required string ValidIssuer { get; set; }
        public required string Secret { get; set; }
        public int ExpiryTime { get; set; }
        public int RefreshTokenExpiryDays { get; set; }
    }
}
