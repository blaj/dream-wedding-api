using AutoMapper;
using DreamWeddingApi.Api.Wedding.DTO;

namespace DreamWeddingApi.Api.Wedding.Mapper;

public class WeddingDetailsDtoMapper : Profile
{
    public WeddingDetailsDtoMapper()
    {
        CreateMap<Entity.Wedding, WeddingDetailsDto>();
    }
}