using ReddNet.Core.Models;

namespace ReddNet.Core.Services.Abstract;

public interface ICommunityService
{
    Task<CommunityModel> GetById(Guid id);
    Task<IEnumerable<CommunityModel>> GetAll();
    Task<CreateCommunityModel> Add(CreateCommunityModel communityModel, string userId);
    Task Delete(Guid id, string userId);
}