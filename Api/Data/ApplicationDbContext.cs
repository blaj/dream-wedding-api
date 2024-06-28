using Microsoft.EntityFrameworkCore;

namespace DreamWeddingApi.Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    public virtual DbSet<Wedding.Entity.Wedding> Weddings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Wedding.Entity.Wedding>().HasQueryFilter(r => !r.Deleted);
    }
}