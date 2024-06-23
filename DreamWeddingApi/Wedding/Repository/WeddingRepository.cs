using DreamWeddingApi.DAL;

namespace DreamWeddingApi.Wedding.Repository;

public class WeddingRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public WeddingRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public IEnumerable<Entity.Wedding> findAll()
    {
        return _applicationDbContext.Weddings.ToList();
    }
}