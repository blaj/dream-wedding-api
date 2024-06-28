using AutoMapper;
using DreamWeddingApi.Api.Wedding.DTO;

namespace DreamWeddingApi.Api.Wedding.Mapper;

public class WeddingMapper : Profile
{

    public WeddingMapper()
    {
        CreateMap<WeddingCreateRequest, Entity.Wedding>();
        CreateMap<WeddingUpdateRequest, Entity.Wedding>();
    }
}