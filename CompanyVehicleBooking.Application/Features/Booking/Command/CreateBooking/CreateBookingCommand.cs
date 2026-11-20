using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Command.CreateBooking
{
    public class CreateBookingCommand :IRequest<CreateBookingCommandResponse>
    {
        public Guid VehicleId { get; set; }
        public DateTime TravelDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string? Destination { get; set; }
        public required string PurposeOfTravel { get; set; }
    }
}
