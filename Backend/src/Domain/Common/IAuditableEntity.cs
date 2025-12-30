namespace DentalHealthSaaS.Backend.src.Domain.Common
{
    /// <summary>
    /// Defines properties for tracking the creation and modification metadata of an entity, including the user
    /// identifiers and timestamps for when the entity was created and last updated.
    /// </summary>
    /// <remarks>Implement this interface to enable auditing features on entities, such as recording who
    /// created or last updated the entity and when these actions occurred. This is commonly used in applications that
    /// require change tracking or historical record keeping.</remarks>
    public interface IAuditableEntity
    {
        Guid CreatedBy { get; set; }
        DateTime CreatedAt { get; set; }

        Guid UpdatedBy { get; set; }
        DateTime UpdatedAt { get; set; }
    }
}
