using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ReddNet.API.Authorization.Requirements;
using ReddNet.Core.Models;
using ReddNet.Domain;
using System.Security.Claims;

namespace ReddNet.API.Authorization.Handlers;

public class IsEligibleForPostDeleteHandler : AuthorizationHandler<IsEligibleForPostDeleteRequirement, PostModel>
{
    private readonly UserManager<User> _userManager;
    public IsEligibleForPostDeleteHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEligibleForPostDeleteRequirement requirement, PostModel resource)
    {
        var communityRole = $"{nameof(Community)}/{resource.PostId}/Admin";
        var moderatorRole = $"{nameof(Community)}/{resource.PostId}/Moderator";
        var currentUser = _userManager.FindByIdAsync(context.User.FindFirstValue("sub")).GetAwaiter().GetResult();

        if (_userManager.IsInRoleAsync(currentUser, communityRole).GetAwaiter().GetResult()
            || _userManager.IsInRoleAsync(currentUser, moderatorRole).GetAwaiter().GetResult()
            || resource.AuthorId.ToString() == currentUser.Id)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}
