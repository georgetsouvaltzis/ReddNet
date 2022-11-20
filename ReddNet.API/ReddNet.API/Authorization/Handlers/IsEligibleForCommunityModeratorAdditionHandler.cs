using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ReddNet.API.Authorization.Requirements;
using ReddNet.Core.Models;
using ReddNet.Domain;
using System.Security.Claims;

namespace ReddNet.API.Authorization.Handlers;

public class IsEligibleForCommunityModeratorAdditionHandler : AuthorizationHandler<IsEligibleForCommunityModeratorAdditionRequirement, CommunityModel>
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public IsEligibleForCommunityModeratorAdditionHandler(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsEligibleForCommunityModeratorAdditionRequirement requirement, CommunityModel resource)
    {
        var communityRoleTemplate = $"{nameof(Community)}/{resource.Id}/Admin";
        var currentUser = _userManager.FindByIdAsync(context.User.FindFirstValue("sub")).GetAwaiter().GetResult();
        if (!_userManager.IsInRoleAsync(currentUser, communityRoleTemplate).GetAwaiter().GetResult())
        {
            context.Fail();
            return Task.CompletedTask;
        }
        context.Succeed(requirement);
        return Task.CompletedTask;
        
    }
}
