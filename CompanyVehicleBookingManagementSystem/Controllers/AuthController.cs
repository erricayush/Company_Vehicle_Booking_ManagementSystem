using CompanyVehicleBooking.Application.Features.Auth.Command.Login;
using CompanyVehicleBooking.Application.Features.Auth.Command.Logout;
using CompanyVehicleBooking.Application.Features.Auth.Command.RefreshToken;
using CompanyVehicleBooking.Application.Features.Auth.Command.Register;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace CompanyVehicleBookingManagementSystem.API.Controllers
{
    [AllowAnonymous]
    public class AuthController(IMediator mediator) : CustomControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("Logout")] 
        public async Task<IActionResult> Logout([FromBody] LogoutCommand command,CancellationToken cancellationToken)
        {
            var result =await _mediator.Send(command,cancellationToken);
            return Ok(result);
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> Refresh(RefreshCommand command,CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}
