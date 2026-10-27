using System.Security.Cryptography;
using ShortLink.Application.Interfaces;
using ShortLink.Domain;
using ShortLink.Infra.Interfaces;

namespace ShortLink.Application.Services;

public class LinkService(ILinkRepository linkRepository) : ILinkService
{
    public async Task<Link> ShortenLink(string url)
    {
        if (!Uri.TryCreate(url, UriKind.Absolute, out var targetUri) ||
            (targetUri.Scheme != Uri.UriSchemeHttp && targetUri.Scheme != Uri.UriSchemeHttps))
        {
            throw new ArgumentException("A valid absolute HTTP or HTTPS URL is required.", nameof(url));
        }

        // The repository uses Link.Id as MongoDB's document identifier.
        // MongoDB enforces uniqueness for this identifier.
        var code = GenerateCode();
        var link = new Link
        {
            Id = code,
            UserId = Guid.Empty,
            Url = targetUri.AbsoluteUri,
            Hash = null,
            Active = true,
            AccesCount = 0
        };

        await linkRepository.CreateLink(link);
        return link;
    }

    public async Task<Link?> GetByCodeAsync(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            return null;

        var link = await linkRepository.GetByCodeAsync(code);
        return link is { Active: true } ? link : null;
    }

    public Task<List<Link>> GetUserLinks(string userId) =>
        linkRepository.GetUserLinks(userId);

    public async Task<Link?> UpdateLink(Link link)
    {
        ArgumentNullException.ThrowIfNull(link);
        var existing = await linkRepository.GetByCodeAsync(link.Id);
        if (existing is null)
            return null;

        await linkRepository.UpdadteLink(link);
        return link;
    }

    public async Task<bool> DeleteLink(Link link)
    {
        ArgumentNullException.ThrowIfNull(link);
        var existing = await linkRepository.GetByCodeAsync(link.Id);
        if (existing is null)
            return false;

        await linkRepository.DeleteLink(link.Id);
        return true;
    }

    private static string GenerateCode()
    {
        const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        Span<byte> bytes = stackalloc byte[8];
        RandomNumberGenerator.Fill(bytes);

        Span<char> code = stackalloc char[bytes.Length];
        for (var i = 0; i < bytes.Length; i++)
            code[i] = alphabet[bytes[i] % alphabet.Length];

        return new string(code);
    }
}
