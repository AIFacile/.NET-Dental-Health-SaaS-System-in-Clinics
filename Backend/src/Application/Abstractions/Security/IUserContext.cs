namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Security
{
    public interface IUserContext
    {
        Guid UserId { get; }
        string UserName { get; }
    }
}
