using DreamWeddingApi.AuthorizationServer.Data;
using DreamWeddingApi.Shared.Common.Entity;

namespace DreamWeddingApi.AuthorizationServer.Repository;

public class UserRepository(ApplicationDbContext dbContext)
{
    public User? FindOneByUsername(string username)
    {
        return dbContext.Users
            .SingleOrDefault(u => u.Username.Equals(username));
    }
}