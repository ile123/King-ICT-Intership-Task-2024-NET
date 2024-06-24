using Api.Data;
using Api.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Api.Repositories.Implementations;

public class AuthRepository(AppDbContext appDbContext) : IAuthRepository
{
    public async Task AddUser(User user)
    {
        appDbContext.Users.Add(user);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await appDbContext
            .Users
            .FirstOrDefaultAsync(x => x.Email == email);
    }
}