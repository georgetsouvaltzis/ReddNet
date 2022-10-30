using System.Text.Json.Serialization;

namespace ReddNet.Core.Models;

public class CreateCommunityModel
{
    [JsonIgnore]
    public Guid CommunityId { get; set; }
    
    public string Name { get; set; }
}
