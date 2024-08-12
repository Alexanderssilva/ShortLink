using MongoDB.Bson;
using MongoDB.Driver;
using ShortLink.Domain;
using ShortLink.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShortLink.Infra.Repositories
{
    public class LinkRepository(MongoContext context) : ILinkRepository
    {
        public async Task CreateLink(Link link) => await context.Links.InsertOneAsync(link);

        public async Task DeleteLink(string linkId)
        {
            var filter = Builders<Link>.Filter.Eq("_id", linkId);
            await context.Links.DeleteOneAsync(filter);
        }

        public async Task<List<Link>> GetUserLinks(string userId)
        {
            var filter = Builders<Link>.Filter.Eq("_id", userId);
            return await context.Links.Find(filter).ToListAsync();
        }

        public async Task UpdadteLink(Link link)
        {
            var filter = Builders<Link>.Filter.Eq("_id", link.Id);
            await context.Links.ReplaceOneAsync(filter,link);
        }


    }
}
