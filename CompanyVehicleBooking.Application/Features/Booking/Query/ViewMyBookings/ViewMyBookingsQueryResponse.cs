namespace CompanyVehicleBooking.Application.Features.Booking.Query.ViewMyBookings
{
    public class ViewMyBookingsQueryResponse
    {
        public Guid VehicleId { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Destination { get; set; }
        public string? Purpose { get; set; }
    }
}
