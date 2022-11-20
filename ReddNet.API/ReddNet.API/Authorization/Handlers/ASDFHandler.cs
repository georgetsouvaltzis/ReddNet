using Microsoft.AspNetCore.Authorization;
using ReddNet.Core.Models;
using System.Security.Claims;

namespace ReddNet.API.Authorization.Handlers;

//public class IsEligibleForCommunityDeleteHandler : AuthorizationHandler<IsEligibleForCommunityDeleteRequirement, Post>
//{
//    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEligibleForCommunityDeleteRequirement requirement, Post resource)
//    {
//        context.Succeed(requirement);
//        return Task.CompletedTask;
//    }
//}

public class ASDFHandler : AuthorizationHandler<ASDFRequirement, PostModel>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ASDFRequirement requirement, PostModel resource)
    {
        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}

public class ASDFRequirement : IAuthorizationRequirement
{

}
