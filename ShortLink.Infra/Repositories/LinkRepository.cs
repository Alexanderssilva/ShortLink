using MongoDB.Driver;
using ShortLink.Domain;
using ShortLink.Infra.Interfaces;

namespace ShortLink.Infra.Repositories;

public class LinkRepository(MongoContext context) : ILinkRepository
{
    public async Task CreateLink(Link link) =>
        await context.Links.InsertOneAsync(link);

    public async Task DeleteLink(string linkId)
    {
        var filter = Builders<Link>.Filter.Eq(link => link.Id, linkId);
        await context.Links.DeleteOneAsync(filter);
    }

    public async Task<List<Link>> GetUserLinks(string userId)
    {
        if (!Guid.TryParse(userId, out var userGuid))
            return [];

        var filter = Builders<Link>.Filter.Eq(link => link.UserId, userGuid);
        return await context.Links.Find(filter).ToListAsync();
    }

    public async Task<Link?> GetByCodeAsync(string code)
    {
        var filter = Builders<Link>.Filter.Eq(link => link.Id, code);
        return await context.Links.Find(filter).FirstOrDefaultAsync();
    }

    public async Task IncrementAccessCountAsync(string code)
    {
        var filter = Builders<Link>.Filter.Eq(link => link.Id, code);
        var update = Builders<Link>.Update.Inc(link => link.AccesCount, 1);
        await context.Links.UpdateOneAsync(filter, update);
    }

    public async Task UpdadteLink(Link link)
    {
        var filter = Builders<Link>.Filter.Eq(item => item.Id, link.Id);
        await context.Links.ReplaceOneAsync(filter, link);
    }
}
