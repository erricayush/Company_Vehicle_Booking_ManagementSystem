using MediatR;

namespace CompanyVehicleBooking.Application.Features.Tracking.Command.CompleteTrip
{
    public class CompleteTripCommand :IRequest<CompleteTripCommandResponse>
    {
        public Guid BookingId { get; set; }
    }
}
