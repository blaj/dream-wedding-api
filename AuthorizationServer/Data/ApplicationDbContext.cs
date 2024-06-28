using Microsoft.EntityFrameworkCore;
using Npgsql.NameTranslation;
using OpenIddict.EntityFrameworkCore.Models;

namespace DreamWeddingApi.AuthorizationServer.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var mapper = new NpgsqlSnakeCaseNameTranslator();

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            for (var type = entityType; type != null; type = type.BaseType)
            {
                if (type.ClrType.Assembly !=
                    typeof(OpenIddictEntityFrameworkCoreApplication).Assembly)
                {
                    continue;
                }

                entityType.SetSchema("open_iddict");
                entityType.SetTableName(mapper.TranslateMemberName(entityType.GetTableName()!));

                break;
            }
        }
    }
}