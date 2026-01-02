namespace DentalHealthSaaS.Backend.src.Domain.Users
{
    public class Role
    {
        public Guid Id { get; set; }
        public required RoleName Name { get; set; }
    }
}
