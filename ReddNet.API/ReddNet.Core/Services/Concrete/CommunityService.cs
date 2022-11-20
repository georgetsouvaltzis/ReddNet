using Microsoft.AspNetCore.Identity;
using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;
using ReddNet.Infrastructure.Repositories;

namespace ReddNet.Core.Services.Concrete;

public class CommunityService : ICommunityService
{
    private readonly IRepositoryAsync<Community> _communityRepository;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public CommunityService(IRepositoryAsync<Community> communityRepository, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        _communityRepository = communityRepository;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<CommunityModel> GetById(Guid id)
    {
        var existingEntity = await _communityRepository.GetByIdAsync(id);

        return new CommunityModel
        {
            Id = existingEntity.Id,
            Name = existingEntity.Name
        };
    }

    public async Task<IEnumerable<CommunityModel>> GetAll()
    {
        var existingCommunities = await _communityRepository.GetAllAsync();

        return existingCommunities.Select(x => new CommunityModel
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public async Task<CreateCommunityModel> Add(CreateCommunityModel createdCommunityModel, string userId)
    {
        var createdCommunity = new Community { Name = createdCommunityModel.Name };
        await _communityRepository.AddAsync(createdCommunity);
        createdCommunityModel.CommunityId = createdCommunity.Id;

        var currentUser = await _userManager.FindByIdAsync(userId);
        await AddToAdminRoleAsync(currentUser, createdCommunity.Id);
        return createdCommunityModel;
    }

    public async Task Delete(Guid id, string userId)
    {
        var currentUser = await _userManager.FindByIdAsync(userId);
        await RemoveCommunityRolesAsync(currentUser, id);
        await _communityRepository.DeleteAsync(id);
    }

    private async Task AddToAdminRoleAsync(User currentUser, Guid communityId)
    {
        var communityRoleTemplate = $"{nameof(Community)}/{communityId}/Admin";

        if (!await _userManager.IsInRoleAsync(currentUser, communityRoleTemplate))
        {
            var createdRole = await _roleManager.CreateAsync(new IdentityRole
            {
                Name = communityRoleTemplate,
            });
            await _userManager.AddToRoleAsync(currentUser, communityRoleTemplate);
        }
    }

    private async Task RemoveCommunityRolesAsync(User currentUser, Guid communityId)
    {
        var roles = new[]
        {
            $"{nameof(Community)}/{communityId}/Admin",
            $"{nameof(Community)}/{communityId}/Moderator"
        };

        foreach (var role in roles)
        {
            var existingUsers = await _userManager.GetUsersInRoleAsync(role);
            var existingRole = await _roleManager.FindByNameAsync(role);
            foreach (var user in existingUsers)
            {
                var task = await _userManager.RemoveFromRoleAsync(user, role);
            }
            await _roleManager.DeleteAsync(existingRole);
        }

    }
}