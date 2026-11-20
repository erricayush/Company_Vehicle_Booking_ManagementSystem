using CompanyVehicleBooking.Application.Features.Booking.Command.ApproveBooking;
using CompanyVehicleBooking.Application.Features.Booking.Command.CreateBooking;
using CompanyVehicleBooking.Application.Features.Booking.Command.RejectBooking;
using CompanyVehicleBooking.Application.Features.Booking.Query.GetAllBooking;
using CompanyVehicleBooking.Application.Features.Booking.Query.GetBookingById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanyVehicleBookingManagementSystem.API.Controllers
{

    public class VehicleBookingController(IMediator mediator) : CustomControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Create-Booking")]
        [Authorize(Roles ="Employee")]
        public async Task<IActionResult> CreateBooking([FromBody]CreateBookingCommand command,CancellationToken cancellationToken)
        {
            var result =await _mediator.Send(command,cancellationToken);
            return Ok(result);
        }


        [HttpGet("Get-All-Bookings")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllBookings([FromQuery]GetAllBookingQuery query,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("Get-Booking-By-Id")]
        [Authorize(Roles ="Admin,Employee")]
        public async Task<IActionResult> GetBookingById([FromQuery]GetBookingByIdQuery query,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Approve-Booking")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ApproveBooking([FromQuery]ApproveBookingCommand command,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Reject-Booking")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> RejectBooking([FromQuery]RejectBookingCommand command,CancellationToken cancellationToken)
        {
            var result =await _mediator.Send(command,cancellationToken);
            return Ok(result);
        }
    }
}
