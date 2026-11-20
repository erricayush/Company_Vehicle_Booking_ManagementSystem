using CompanyVehicleBooking.Domain.Enums;

namespace CompanyVehicleBooking.Domain.Entities
{
    public class Vehicle
    {
        public Guid Id { get; set; }
        public string? RegistrationNumber { get; set; }
        public string? VehicleName { get; set; }
        public VehicleType VehicleType { get; set; }
        public int SeatingCapacity { get; set; }
        public bool IsActive { get; set; }
        public List<VehicleBooking> VehicleBookings { get; set; } = [];
    }
}
