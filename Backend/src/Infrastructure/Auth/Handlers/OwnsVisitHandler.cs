using DentalHealthSaaS.Backend.src.Application.Abstractions.Security;
using DentalHealthSaaS.Backend.src.Application.Security.Requirements;
using DentalHealthSaaS.Backend.src.Domain.Visits;
using Microsoft.AspNetCore.Authorization;

namespace DentalHealthSaaS.Backend.src.Infrastructure.Auth.Handlers
{
    public class OwnsVisitHandler(IUserContext user) : AuthorizationHandler<OwnsVisitRequirement, Visit>
    {
        private readonly IUserContext _user = user;

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OwnsVisitRequirement requirement,
            Visit resource)
        {
            if (resource.DoctorId == _user.UserId)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
