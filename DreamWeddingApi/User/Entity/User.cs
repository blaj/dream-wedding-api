using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DreamWeddingApi.Common.Entity;

namespace DreamWeddingApi.User.Entity;

[Table("users", Schema = "users")]
public class User : AuditingEntity
{
    [Column("username")]
    [Required]
    public String Username { get; set; } = string.Empty;

    [Column("email")] [Required] public String Email { get; set; } = string.Empty;

}