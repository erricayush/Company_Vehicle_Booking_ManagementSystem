namespace CompanyVehicleBooking.Application.ServicesContracts
{
    public interface ICurrentUser
    {
        public string? Id { get; }
        public string? Email { get; }
        public string? Role { get; }

        bool IsInRole(string v);
    }
}
