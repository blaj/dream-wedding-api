using System.Security.Claims;
using DreamWeddingApi.AuthorizationServer.Repository;
using DreamWeddingApi.Shared.Common.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace DreamWeddingApi.AuthorizationServer.Service;

public class AuthenticationService(UserRepository userRepository)
{
    public ClaimsPrincipal? GetPrincipal(string username, string password)
    {
        var user = GetUser(username, password);

        if (user is null)
        {
            return null;
        }
        
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Email, user.Email)
        };

        return new ClaimsPrincipal(
            new List<ClaimsIdentity>
            {
                new(claims, CookieAuthenticationDefaults.AuthenticationScheme)
            });
    }

    private User? GetUser(string username, string password)
    {
        var user = userRepository.FindOneByUsername(username);

        if (user is null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            return null;
        }

        return user;
    }
}