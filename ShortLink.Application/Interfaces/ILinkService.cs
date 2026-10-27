using ShortLink.Domain;

namespace ShortLink.Application.Interfaces;

public interface ILinkService
{
    Task<List<Link>> GetUserLinks(string userId);
    Task<Link> ShortenLink(string url);
    Task<Link?> UpdateLink(Link link);
    Task<bool> DeleteLink(Link link);
    Task<Link?> GetByCodeAsync(string code);
    Task RegisterAccessAsync(string code);
}
