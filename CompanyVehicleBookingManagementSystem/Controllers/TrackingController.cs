using CompanyVehicleBooking.Application.Features.Booking.Query.ViewMyBookings;
using CompanyVehicleBooking.Application.Features.Tracking.Command;
using CompanyVehicleBooking.Application.Features.Tracking.Command.CompleteTrip;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyVehicleBookingManagementSystem.API.Controllers
{

    public class TrackingController(IMediator mediator) : CustomControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPut("Allocate-Vehicle")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllocateVehicle([FromBody]AllocateVehicleCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Complete-Trip")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CompleteTrip([FromQuery]CompleteTripCommand command, CancellationToken cancellationToken)
        {
            var result =await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpGet("View-My-Bookings")]
        [Authorize(Roles ="Employee")]
        public async Task<IActionResult> ViewMyBookings([FromQuery]ViewMyBookingsQuery query,CancellationToken cancellationToken)
        {
            var result =await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }
    }
}
