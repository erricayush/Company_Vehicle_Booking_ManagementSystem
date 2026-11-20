using CompanyVehicleBooking.Domain.Enums;

namespace CompanyVehicleBooking.Domain.Entities
{
    public class VehicleBooking
    {
        public Guid Id { get; set; }
        public Guid VehicleId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Destination { get; set; }
        public string? Purpose { get; set; }
        public BookingStatus BookingStatus { get; set; }
        public DateTime CreatedDate { get; set; }
        public Vehicle Vehicle { get; set; } = null!;
        public List<BookingStatusHistory> BookingStatusHistories { get; set; } = [];
        public ApplicationUser Employee { get; set; } = null!;

    }
}
