using MongoDB.Driver;
using ShortLink.Domain;
using ShortLink.Infra.Interfaces;

namespace ShortLink.Infra.Repositories;

public class UserRepository(MongoContext context) : IUserRepository
{
    public async Task InsertUserAsync(User user) =>
        await context.Users.InsertOneAsync(user);

    public async Task<User?> GetUserAsync(string email) =>
        await context.Users.Find(u => u.Email == email).FirstOrDefaultAsync();
}
