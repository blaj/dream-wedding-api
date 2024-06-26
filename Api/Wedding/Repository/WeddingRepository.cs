using DreamWeddingApi.Api.DAL;

namespace DreamWeddingApi.Api.Wedding.Repository;

public class WeddingRepository(ApplicationDbContext applicationDbContext)
{
    public IEnumerable<Api.Wedding.Entity.Wedding> FindAll()
    {
        return applicationDbContext.Weddings.ToList();
    }
}