using ReddNet.Core.Models;

namespace ReddNet.Core.Services.Abstract;

public interface ICommunityService
{
    Task<CommunityModel> GetById(Guid id);
    Task<IEnumerable<CommunityModel>> GetAll();
}