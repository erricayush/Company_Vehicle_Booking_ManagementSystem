using CompanyVehicleBooking.Application.ServicesContracts;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CompanyVehicleBooking.Application.Services
{
    public class CurrentUser(IHttpContextAccessor httpContextAccessor) : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public string?  Id => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        public string? Email=> _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;
       public string? Role => _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }
    }
}


