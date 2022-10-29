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
    
    public async Task<IEnumerable<CommunityModel>> GetAll()
    {
        var existingCommunities = await _communityRepository.GetAllAsync();

        return existingCommunities.Select(x => new CommunityModel
        {
            Id = x.Id,
            Name = x.Name,
        });
    }

    public async Task<CreateCommunityModel> Add(CreateCommunityModel createdCommunityModel)
    {
        var createdCommunity = new Community { Name = createdCommunityModel.Name };
        await _communityRepository.AddAsync(createdCommunity);
        return createdCommunityModel;
    }

    public async Task Delete(Guid id)
    {
        await _communityRepository.DeleteAsync(id);
    }
}
