using DreamWeddingApi.Api.Wedding.Repository;

namespace DreamWeddingApi.Api.Wedding.Service;

public class WeddingService(WeddingRepository weddingRepository)
{
    public IEnumerable<Api.Wedding.Entity.Wedding> GetList()
    {
        return weddingRepository.FindAll();
    }
}