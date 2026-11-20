using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Query.GetBookingById
{
    public class GetBookingByIdQuery :IRequest<GetBookingByIdQueryResponse>
    {
        public Guid BookingId { get; set; }
    }
}
