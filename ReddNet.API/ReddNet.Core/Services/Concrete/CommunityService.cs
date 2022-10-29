using ReddNet.Core.Models;
using ReddNet.Core.Services.Abstract;
using ReddNet.Domain;
using ReddNet.Infrastructure.Repositories;

namespace ReddNet.Core.Services.Concrete;

public class CommunityService : ICommunityService
{
    private readonly IRepositoryAsync<Community> _communityRepository;
    public CommunityService(IRepositoryAsync<Community> communityRepository)
    {
        _communityRepository = communityRepository;
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
}
