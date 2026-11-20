using CompanyVehicleBooking.Application.Behaviour;
using CompanyVehicleBooking.Application.Features.Auth.Command.Register;
using CompanyVehicleBooking.Application.Options;
using CompanyVehicleBooking.Application.Services;
using CompanyVehicleBooking.Application.ServicesContracts;
using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Domain.RepositoryContracts;
using CompanyVehicleBooking.Infrastructure.Context;
using CompanyVehicleBooking.Infrastructure.Repository;
using CompanyVehicleBooking.Infrastructure.Seed;
using CompanyVehicleBookingManagementSystem.API.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
                          options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "JWR Authorization header using the bearer scheme"
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddTransient<ICurrentUser,CurrentUser>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IVehicleBookingService, VehicleBookingService>();
builder.Services.AddScoped<IVehicleBookingRepository,VehicleBookingRepository>();
builder.Services.AddScoped<IVehicleRepository,VehicleRepository>();
builder.Services.AddScoped<IBookingStatusHistoryRepository, BookingStatusHistoryRepository>();

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddScoped<ITokenService, TokenService>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(RegisterCommandHandler).Assembly);
    cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(typeof(RegisterCommandValidator).Assembly);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));

    var policy = new AuthorizationPolicyBuilder()
                 .RequireAuthenticatedUser()
                 .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});


builder.Services.AddIdentity<ApplicationUser,IdentityRole<Guid>>()
                 .AddRoles<IdentityRole<Guid>>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

// Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}
 )
   .AddJwtBearer(options =>
   {
       options.SaveToken = true;
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters = new TokenValidationParameters
       {
           ValidateIssuer = true,
           ValidateAudience = true,
           ValidAudience = builder.Configuration["JWT:ValidAudience"],
           ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
           ClockSkew = TimeSpan.Zero,
           IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
       };
       options.Events = new JwtBearerEvents
       {
           OnTokenValidated = async context =>
           {
               var userManager =
                   context.HttpContext.RequestServices
                       .GetRequiredService<UserManager<ApplicationUser>>();

               var userId = context.Principal?
                   .FindFirst(ClaimTypes.NameIdentifier)?.Value;

               var tokenVersion = context.Principal?
                   .FindFirst("TokenVersion")?.Value;

               if (userId == null || tokenVersion == null)
               {
                   context.Fail("Invalid token.");
                   return;
               }

               var user = await userManager.FindByIdAsync(userId);

               if (user == null ||
                   user.TokenVersion.ToString() != tokenVersion)
               {
                   context.Fail("Token has been revoked.");
               }
           },


           OnChallenge = async context =>
           {
               context.HandleResponse();
               context.Response.StatusCode = StatusCodes.Status401Unauthorized;
               context.Response.ContentType = "application/json";
               await context.Response.WriteAsJsonAsync(new
               {
                   StatusCode = 401,
                   Message = "you are not authorized please login with valid token"
               });
           }
       };
   }
    );

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/Swagger/v1/swagger.json", "Company Vehicle Booking Management System");
    c.RoutePrefix = string.Empty;
    c.DocumentTitle = "Company Vehicle Booking Management System";
});

app.UseExceptionHandler();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
await DbSeeder.SeedData(app);

app.Run();
