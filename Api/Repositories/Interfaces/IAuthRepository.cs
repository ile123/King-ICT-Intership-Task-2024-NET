using Models.Entities;

namespace Api.Repositories.Interfaces;

public interface IAuthRepository
{
    Task AddUser(User user);
    Task<User?> GetUserByEmail(string email);
}