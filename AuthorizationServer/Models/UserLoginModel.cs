using System.ComponentModel.DataAnnotations;

namespace DreamWeddingApi.AuthorizationServer.Models;

public class UserLoginModel
{
    [Required] public string Username { get; set; } = string.Empty;

    [Required] public string Password { get; set; } = string.Empty;
    
    public string? ReturnUrl { get; set; }
    
    
}