using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileSystemGlobbing.Internal.PatternContexts;
using ReddNet.API.Authorization.Requiremenets;
using ReddNet.Core.Models;
using ReddNet.Domain;
using System.Security.Claims;

namespace ReddNet.API.Authorization.Handlers;

public class IsEligibleForCommunityDeleteHandler : AuthorizationHandler<IsEligibleForCommunityDeleteRequirement, CommunityModel>
{
    private readonly UserManager<User> _userManager;
    public IsEligibleForCommunityDeleteHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEligibleForCommunityDeleteRequirement requirement, CommunityModel resource)
    {
        var communityRoleTemplate = $"{nameof(Community)}/{resource.Id}/Admin";
        var user = _userManager.FindByIdAsync(context.User.FindFirstValue("sub")).GetAwaiter().GetResult();
        
        if (_userManager.IsInRoleAsync(user,communityRoleTemplate).GetAwaiter().GetResult())
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
        // TODO: Add Custom AuthorizationFailureReason?
        context.Fail();
        return Task.CompletedTask;
    }
}



//public class IsEligibleForCommunityDeleteHandler : AuthorizationHandler<ASDFRequirement, PostModel>
//{
//    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ASDFRequirement requirement, PostModel resource)
//    {
//        context.Succeed(requirement);
//        return Task.CompletedTask;
//    }
//}

//public class ASDFRequirement : IAuthorizationRequirement
//{

//}
