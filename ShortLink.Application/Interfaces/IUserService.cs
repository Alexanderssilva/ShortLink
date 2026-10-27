using ShortLink.Domain;

namespace ShortLink.Application.Interfaces;

public interface IUserService
{
    Task<User?> AuthenticateUserAsync(string email, string password);
    Task<bool> InsertUserAsync(string email, string password);
}
