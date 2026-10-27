using ShortLink.Domain;

namespace ShortLink.Infra.Interfaces;

public interface ILinkRepository
{
    Task<List<Link>> GetUserLinks(string userId);
    Task<Link?> GetByCodeAsync(string code);
    Task UpdadteLink(Link link);
    Task DeleteLink(string linkId);
    Task CreateLink(Link link);
}
