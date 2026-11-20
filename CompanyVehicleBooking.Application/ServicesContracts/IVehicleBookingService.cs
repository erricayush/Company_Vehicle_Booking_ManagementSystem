using CompanyVehicleBooking.Application.Features.Booking.Command.ApproveBooking;
using CompanyVehicleBooking.Application.Features.Booking.Command.CreateBooking;
using CompanyVehicleBooking.Application.Features.Booking.Command.RejectBooking;
using CompanyVehicleBooking.Application.Features.Booking.Query.GetAllBooking;
using CompanyVehicleBooking.Application.Features.Booking.Query.GetBookingById;
using CompanyVehicleBooking.Application.Features.Booking.Query.ViewMyBookings;
using CompanyVehicleBooking.Application.Features.Tracking.Command;
using CompanyVehicleBooking.Application.Features.Tracking.Command.CompleteTrip;

namespace CompanyVehicleBooking.Application.ServicesContracts
{
    public interface IVehicleBookingService
    {
        Task<AllocateVehicleCommandResponse> AllocateVehicleAsync(AllocateVehicleCommand request, CancellationToken cancellationToken);
        Task<ApproveBookingCommandResponse> ApproveBookingAsync(ApproveBookingCommand request, CancellationToken cancellationToken);
        Task<CompleteTripCommandResponse> CompleteTripAsync(CompleteTripCommand request, CancellationToken cancellationToken);
        Task<CreateBookingCommandResponse> CreateBookingAsync(CreateBookingCommand request, CancellationToken cancellationToken);
        Task<List<GetAllBookingQueryResponse>> GetAllBookingAsync(GetAllBookingQuery request, CancellationToken cancellationToken);
        Task<GetBookingByIdQueryResponse> GetBookingByIdAsync(GetBookingByIdQuery request, CancellationToken cancellationToken);
        Task<RejectBookingCommandResponse> RejectBookingAsync(RejectBookingCommand request, CancellationToken cancellationToken);
        Task<List<ViewMyBookingsQueryResponse>> ViewMyBookingsAsync(ViewMyBookingsQuery request, CancellationToken cancellationToken);
    }
}
