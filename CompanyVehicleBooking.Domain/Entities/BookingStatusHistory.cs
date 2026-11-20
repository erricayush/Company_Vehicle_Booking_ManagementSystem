using CompanyVehicleBooking.Domain.Enums;

namespace CompanyVehicleBooking.Domain.Entities
{
    public class BookingStatusHistory
    {
        public Guid Id { get; set; }
        public Guid VehicleBookingId { get; set; }
        public Status Status { get; set; }
        public DateTime UpdatedDate { get; set; }
        public VehicleBooking VehicleBooking { get; set; } = null!;
    }
}
