using MediatR;

namespace CompanyVehicleBooking.Application.Features.Tracking.Command
{
    public class AllocateVehicleCommand :IRequest<AllocateVehicleCommandResponse>
    {
        public Guid VehicleId { get; set; }
        public Guid BookingId { get; set; }
    }
}
