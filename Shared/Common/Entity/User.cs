using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DreamWeddingApi.Shared.Common.Entity;

[Table("users", Schema = "users")]
public class User : AuditingEntity
{
    [Column("username")]
    [Required]
    public string Username { get; set; } = string.Empty;

    [Column("email")] [Required] public string Email { get; set; } = string.Empty;

}