using CompanyVehicleBooking.Application.Features.Auth.Command.Login;
using CompanyVehicleBooking.Application.Features.Auth.Command.Logout;
using CompanyVehicleBooking.Application.Features.Auth.Command.RefreshToken;
using CompanyVehicleBooking.Application.Features.Auth.Command.Register;
using CompanyVehicleBooking.Application.Options;
using CompanyVehicleBooking.Application.ServicesContracts;
using CompanyVehicleBooking.Domain.Constants;
using CompanyVehicleBooking.Domain.CustomExceptions;
using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Infrastructure.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CompanyVehicleBooking.Application.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,
                 RoleManager<IdentityRole<Guid>> roleManager,
                 ILogger<AuthService> logger,
                 ITokenService tokenService, AppDbContext context, SignInManager<ApplicationUser> signInManager, IHttpContextAccessor httpContextAccessor,  IOptions<JwtOptions> options) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly ITokenService _tokenService = tokenService; 
        private readonly AppDbContext _context = context;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly JwtOptions _jwtOption = options.Value;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        public async Task<LoginCommandResponse> LoginAsync(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email) ?? throw new BadRequestException("User with this Email is not registered with us.");

            bool isValidPassword = await _userManager.CheckPasswordAsync(user, request.Password);
            if (isValidPassword == false)
            {
               throw new  UnauthorizedException("You are not authorized or incorrect password.");
            }

            List<Claim> authClaims = [
                    new (ClaimTypes.NameIdentifier,user.Id.ToString()),
                    new (ClaimTypes.Email,user.Email!),
                    new (ClaimTypes.Name, user.UserName),
                    new Claim("TokenVersion", user.TokenVersion.ToString()),
                    new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
             ];

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var userRole in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            }

            var token = _tokenService.GenerateAccessToken(authClaims);

            string refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOption.RefreshTokenExpiryDays);
           


            var updatResult = await _userManager.UpdateAsync(user);
            if (!updatResult.Succeeded)
            {
           
                var errors = updatResult.Errors.Select(e => e.Description);
                _logger.LogError($"Failed to update user , Errors : {string.Join(",", errors)}");
                throw new BadRequestException("Error in updating user");

            }

            await _context.SaveChangesAsync(cancellationToken);

            return new LoginCommandResponse
            {
                AccessToken = token,
                RefreshToken = refreshToken
            };
        }

        public async Task<LogoutCommandResponse> LogoutAsync( LogoutCommand request,CancellationToken cancellationToken)
        {
            var username = _httpContextAccessor
                .HttpContext?
                .User
                .Identity?
                .Name;

            if (string.IsNullOrEmpty(username))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var user = await _userManager.FindByNameAsync(username);

            if (user == null)
            {
                throw new UnauthorizedAccessException("User not found.");
            }

            user.RefreshToken = string.Empty;
            user.RefreshTokenExpiryTime = DateTime.MinValue;
            user.TokenVersion++;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Failed to logout user.");
            }

            await _signInManager.SignOutAsync();

            return new LogoutCommandResponse
            {
                Message = "Logout Successfully"
            };
        }


        public async Task<RefreshCommandResponse> RefreshAsync(RefreshCommand request, CancellationToken cancellationToken)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
            var username = principal.Identity.Name;

            if (string.IsNullOrWhiteSpace(username))
            {
                throw new BadRequestException("Token is expired or not present please login again");
            }

            var user = await _userManager.FindByEmailAsync(username);
            if (user == null)
            {
                throw new NotFoundException("User is not found please login again");
            }

            if (user.RefreshToken !=request.RefreshToken || user.RefreshTokenExpiryTime<=DateTime.UtcNow)
            {
                throw new BadRequestException("Invalid or expired refresh token");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();


            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtOption.RefreshTokenExpiryDays);

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync(cancellationToken);
            

            return new RefreshCommandResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }



        public async Task<RegisterCommandResponse> RegisterAsync(RegisterCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new BadRequestException("User already exists");
            }

            if ((await _roleManager.RoleExistsAsync(Roles.Employee)) == false)
            {
                var roleResult = await _roleManager
                      .CreateAsync(new IdentityRole<Guid>(Roles.Employee));

                if (roleResult.Succeeded == false)
                {
                    var roleErros = roleResult.Errors.Select(e => e.Description);
                    _logger.LogError($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                    throw new BadRequestException($"Failed to create user role. Errors : {string.Join(",", roleErros)}");
                }
            }

            ApplicationUser user = new()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.Email,
                EmailConfirmed = true
            };


            var createUserResult = await _userManager.CreateAsync(user, request.Password);

            if (createUserResult.Succeeded == false)
            {
                var errors = createUserResult.Errors.Select(e => e.Description);
                _logger.LogError(
                    $"Failed to create user. Errors: {string.Join(", ", errors)}"
                );
                throw new BadRequestException($"Failed to create user. Errors: {string.Join(", ", errors)}");
            }


            var addUserToRoleResult = await _userManager.AddToRoleAsync(user: user, role: Roles.Employee);

            if (addUserToRoleResult.Succeeded == false)
            {
                var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                _logger.LogError($"Failed to add role to the user. Errors : {string.Join(",", errors)}");
            }

            return new RegisterCommandResponse { Message = "Registered Successfully" };
        }
    }
}
