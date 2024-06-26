using System.ComponentModel.DataAnnotations.Schema;

namespace DreamWeddingApi.Shared.Common.Entity;

public class AuditingEntity : IdEntity
{
    [Column("created_at")] public DateTime CreatedAt = DateTime.Now;

    [Column("updated_at")] public DateTime? UpdatedAt;

    [Column("deleted_at")] public DateTime? DeletedAt;

    [Column("deleted")] public bool Deleted = false;
}