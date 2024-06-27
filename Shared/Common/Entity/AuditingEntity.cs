using System.ComponentModel.DataAnnotations.Schema;

namespace DreamWeddingApi.Shared.Common.Entity;

public class AuditingEntity : IdEntity
{
    [Column("created_at")] public DateTime CreatedAt { get; set; } = DateTime.Now;

    [Column("updated_at")] public DateTime? UpdatedAt { get; set; }

    [Column("deleted_at")] public DateTime? DeletedAt { get; set; }

    [Column("deleted")] public bool Deleted { get; set; } = false;
}