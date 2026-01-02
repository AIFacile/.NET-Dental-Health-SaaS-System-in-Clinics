using Microsoft.AspNetCore.Authorization;

namespace DentalHealthSaaS.Backend.src.Application.Security.Requirements
{
    public class OwnsVisitRequirement : IAuthorizationRequirement
    {
    }
}
