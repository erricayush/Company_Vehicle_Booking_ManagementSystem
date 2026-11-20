using CompanyVehicleBooking.Domain.CustomExceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CompanyVehicleBookingManagementSystem.API.Middleware
{
    public class AppExceptionHandler(ILogger<AppExceptionHandler> logger) : IExceptionHandler
    {
        private readonly ILogger<AppExceptionHandler> _logger = logger;

        public async ValueTask<bool> TryHandleAsync(
         HttpContext httpContext,
         Exception exception,
         CancellationToken cancellationToken)
        {
            if (exception is ValidationException validationException)
            {
                _logger.LogWarning("Validation failed: {Message}", validationException.Message);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Title = "One or more validation errors occurred.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    Instance = httpContext.Request.Path
                };

                problemDetails.Extensions["errors"] = validationException.Errors
                    .Select(error => error.ErrorMessage)
                    .ToList();

                httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
                return true;
            }
            var(statusCode,message)=MapException(exception);
            var customProblemDetails = new ProblemDetails
            {
                Status=(int)statusCode,
                Title=message,
                Instance = httpContext.Request.Path
            };
            httpContext.Response.StatusCode=(int)statusCode;
            await httpContext.Response.WriteAsJsonAsync(customProblemDetails, cancellationToken);
            return true;


        }

        private static (HttpStatusCode statusCode, string message) MapException(Exception exception)
        {
            return exception switch
            {
                BadRequestException => (HttpStatusCode.BadRequest, exception.Message),
                NotFoundException => (HttpStatusCode.NotFound, exception.Message),
                UnauthorizedException => (HttpStatusCode.Unauthorized, exception.Message),
                _ => (HttpStatusCode.InternalServerError, "An unexpected error occurred"),
            };


        }
    }
}
