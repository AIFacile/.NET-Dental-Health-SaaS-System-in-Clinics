namespace DentalHealthSaaS.Backend.src.Domain.Common
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}
