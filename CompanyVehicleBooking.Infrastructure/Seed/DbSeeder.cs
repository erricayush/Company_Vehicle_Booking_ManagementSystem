using CompanyVehicleBooking.Domain.Constants;
using CompanyVehicleBooking.Domain.Entities;
using CompanyVehicleBooking.Domain.Enums;
using CompanyVehicleBooking.Infrastructure.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CompanyVehicleBooking.Infrastructure.Seed
{
    public class DbSeeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>();
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            string[] roles =
            {
                Roles.Admin,
                Roles.Employee
            };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>
                    {
                        Name = role
                    });
                }
            }

            if (userManager.Users.Any() == false)
            {
                var user = new ApplicationUser
                {
                    Name = "Admin",
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    EmailConfirmed = true,
                    SecurityStamp = Guid.NewGuid().ToString()
                };


                if ((await roleManager.RoleExistsAsync(Roles.Admin)) == false)
                {
                    logger.LogInformation("Admin role is creating");
                    var roleResult = await roleManager
                      .CreateAsync(new IdentityRole<Guid>(Roles.Admin));

                    if (roleResult.Succeeded == false)
                    {
                        var roleErros = roleResult.Errors.Select(e => e.Description);
                        logger.LogError($"Failed to create admin role. Errors : {string.Join(",", roleErros)}");

                        return;
                    }
                    logger.LogInformation("Admin role is created");
                }

                var createUserResult = await userManager
                      .CreateAsync(user: user, password: "Admin@123");

                if (createUserResult.Succeeded == false)
                {
                    var errors = createUserResult.Errors.Select(e => e.Description);
                    logger.LogError(
                        $"Failed to create admin user. Errors: {string.Join(", ", errors)}"
                    );
                    return;
                }

                var addUserToRoleResult = await userManager
                                .AddToRoleAsync(user: user, role: Roles.Admin);

                if (addUserToRoleResult.Succeeded == false)
                {
                    var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                    logger.LogError($"Failed to add admin role to user. Errors : {string.Join(",", errors)}");
                }
                logger.LogInformation("Admin user is created");
            }


            var userEmployee = await userManager.FindByEmailAsync("Employee@gmail.com");
            if (userEmployee == null)
            {
                var user = new ApplicationUser
                {
                    UserName = "Employee@gmail.com",
                    Email = "Employee@gmail.com",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(user, "Employee@123");
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description);
                    logger.LogError($"Failed to update user , Errors : {string.Join(",", errors)}");
                    throw new Exception($"Failed to update user , Errors : {string.Join(",", errors)}");
                }

                if (!await userManager.IsInRoleAsync(user, Roles.Employee))
                {
                    await userManager.AddToRoleAsync(user, Roles.Employee);
                }
            }



            if (!context!.Vehicles.Any())
            {
                var vehicles = new List<Vehicle>
                {
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        RegistrationNumber = "abc-12",
                        VehicleName = "amaze",
                        VehicleType = VehicleType.Car,
                        SeatingCapacity = 6,
                        IsActive = true
                    },
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        RegistrationNumber = "abc-13",
                        VehicleName = "hunter",
                        VehicleType = VehicleType.Bike,
                        SeatingCapacity = 2,
                        IsActive = true
                    },
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        RegistrationNumber = "abc-14",
                        VehicleName = "Activa",
                        VehicleType = VehicleType.Bike,
                        SeatingCapacity = 2,
                        IsActive = true
                    },
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        RegistrationNumber = "abc-15",
                        VehicleName = "swift",
                        VehicleType = VehicleType.Bike,
                        SeatingCapacity = 4,
                        IsActive = true
                    },
                    new ()
                    {
                        Id = Guid.NewGuid(),
                        RegistrationNumber = "abc-16",
                        VehicleName = "thar",
                        VehicleType = VehicleType.Car,
                        SeatingCapacity = 5,
                        IsActive = false
                    },
                };

                await context.Vehicles.AddRangeAsync(vehicles);
                await context.SaveChangesAsync();
            }



        }
    }
}

