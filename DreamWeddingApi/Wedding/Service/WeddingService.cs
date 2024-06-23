using DreamWeddingApi.Wedding.Repository;

namespace DreamWeddingApi.Wedding.Service;

public class WeddingService
{
    private readonly WeddingRepository _weddingRepository;

    public WeddingService(WeddingRepository weddingRepository)
    {
        _weddingRepository = weddingRepository;
    }

    public IEnumerable<Entity.Wedding> getList()
    {
        return _weddingRepository.findAll();
    }
}