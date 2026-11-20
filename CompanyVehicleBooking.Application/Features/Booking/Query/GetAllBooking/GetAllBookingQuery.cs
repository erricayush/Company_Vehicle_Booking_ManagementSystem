using MediatR;

namespace CompanyVehicleBooking.Application.Features.Booking.Query.GetAllBooking
{
    public class GetAllBookingQuery :IRequest<List<GetAllBookingQueryResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Search {  get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
        
    }
}
