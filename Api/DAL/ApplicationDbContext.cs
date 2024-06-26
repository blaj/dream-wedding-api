using Microsoft.EntityFrameworkCore;

namespace DreamWeddingApi.Api.DAL;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public virtual DbSet<Wedding.Entity.Wedding> Weddings { get; set; }
}