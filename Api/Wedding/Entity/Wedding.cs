using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DreamWeddingApi.Shared.Common.Entity;

namespace DreamWeddingApi.Api.Wedding.Entity;

[Table("wedding", Schema = "wedding")]
public class Wedding : AuditingEntity
{
    [Column("name")]
    [Required]
    public string Name { get; set; } = string.Empty;
    
    [Column("on_date")]
    [Required]
    public DateTime OnDate { get; set; }

    [Column("budget")]
    [Required]
    public int Budget { get; set; } = 0;
}