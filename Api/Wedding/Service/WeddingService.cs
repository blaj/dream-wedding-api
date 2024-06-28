using AutoMapper;
using DreamWeddingApi.Api.Wedding.DTO;
using DreamWeddingApi.Api.Wedding.Repository;
using DreamWeddingApi.Shared.Common.Exception;

namespace DreamWeddingApi.Api.Wedding.Service;

public class WeddingService(WeddingRepository weddingRepository, IMapper mapper)
{
    public IEnumerable<WeddingListItemDto> GetList()
    {
        return weddingRepository.FindAll().Select(mapper.Map<WeddingListItemDto>);
    }

    public WeddingDetailsDto? GetOne(long id)
    {
        return mapper.Map<WeddingDetailsDto>(weddingRepository.FindById(id));
    }

    public Entity.Wedding Create(WeddingCreateRequest weddingCreateRequest)
    {
        return weddingRepository.Add(mapper.Map<Entity.Wedding>(weddingCreateRequest), true);
    }

    public void Update(long id, WeddingUpdateRequest weddingUpdateRequest)
    {
        var wedding = FetchWedding(id);

        weddingRepository.Update(mapper.Map(weddingUpdateRequest, wedding), true);
    }
    
    public void Delete(long id)
    {
        weddingRepository.Delete(FetchWedding(id), true);
    }

    private Entity.Wedding FetchWedding(long id)
    {
        return
            weddingRepository.FindById(id)
            ??
            throw new EntityNotFoundException("Wedding not found");
    }
}