namespace DentalHealthSaaS.Backend.src.Application.Abstractions.Security
{
    /// <summary>
    /// Represents contextual information about the current user.
    /// </summary>
    public interface IUserContext
    {
        Guid UserId { get; }
        string UserName { get; }
        bool HasUser {  get; }
    }
}
