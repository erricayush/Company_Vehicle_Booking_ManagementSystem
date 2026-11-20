using Microsoft.AspNetCore.Identity;

namespace CompanyVehicleBooking.Domain.Entities
{
    public class ApplicationUser :IdentityUser<Guid>
    {
        public string? Name { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpiryTime { get; set; }
        public int TokenVersion { get; set; } = 0;
        public List<VehicleBooking> VehicleBookings { get; set; } = [];
    }
}
