using DreamWeddingApi.Api.Data;
using DreamWeddingApi.Shared.Common.Repository;

namespace DreamWeddingApi.Api.Wedding.Repository;

public class WeddingRepository(ApplicationDbContext dbContext)
    : AuditingEntityRepository<Entity.Wedding>(dbContext)
{
}