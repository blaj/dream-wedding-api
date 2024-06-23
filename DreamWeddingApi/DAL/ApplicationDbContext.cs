using Microsoft.EntityFrameworkCore;

namespace DreamWeddingApi.DAL;

public class ApplicationDbContext : DbContext
{
    public virtual DbSet<User.Entity.User> Users { get; set; }
    public virtual DbSet<Wedding.Entity.Wedding> Weddings { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}