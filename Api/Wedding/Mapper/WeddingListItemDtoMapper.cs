using AutoMapper;
using DreamWeddingApi.Api.Wedding.DTO;

namespace DreamWeddingApi.Api.Wedding.Mapper;

public class WeddingListItemDtoMapper : Profile
{
    public WeddingListItemDtoMapper()
    {
        CreateMap<Entity.Wedding, WeddingListItemDto>();
    }
}