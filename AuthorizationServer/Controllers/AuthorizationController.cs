using System.Security.Claims;
using DreamWeddingApi.AuthorizationServer.Service;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace DreamWeddingApi.AuthorizationServer.Controllers;

[ApiController]
public class AuthorizationController(
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictScopeManager scopeManager,
    AuthorizationService authService) : Controller
{
    [HttpGet("~/oauth/authorize")]
    [HttpPost("~/oauth/authorize")]
    public async Task<IActionResult> Authorize()
    {
        var request =
            HttpContext.GetOpenIddictServerRequest()
            ??
            throw new InvalidOperationException(
                "The OpenID Connect request cannot be retrieved.");

        var parameters = authService.ParseOAuthParameters(HttpContext,
            new List<string> { OpenIddictConstants.Parameters.Prompt });

        var result =
            await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        if (!authService.IsAuthenticated(result, request))
        {
            return Challenge(properties: new AuthenticationProperties
            {
                RedirectUri = authService.BuildRedirectUrl(HttpContext.Request, parameters)
            }, [CookieAuthenticationDefaults.AuthenticationScheme]);
        }

        var clientId =
            request.ClientId
            ??
            throw new InvalidOperationException("Cannot get client name from request");

        var application =
            await applicationManager.FindByClientIdAsync(clientId)
            ??
            throw new InvalidOperationException(
                "Details concerning the calling client application cannot be found.");

        var consentType = await applicationManager.GetConsentTypeAsync(application);

        if (consentType != OpenIddictConstants.ConsentTypes.Explicit)
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                        OpenIddictConstants.Errors.InvalidClient,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Only explicit consent clients are supported"
                }));
        }

        var userId =
            result.Principal?.FindFirst(ClaimTypes.Name)!.Value;

        var identity =
            new ClaimsIdentity(
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: OpenIddictConstants.Claims.Name,
                roleType: OpenIddictConstants.Claims.Role);

        identity
            .SetClaim(OpenIddictConstants.Claims.Subject, userId)
            .SetClaim(OpenIddictConstants.Claims.Email, userId)
            .SetClaim(OpenIddictConstants.Claims.Name, userId)
            .SetClaims(
                OpenIddictConstants.Claims.Role,
                [..new List<string> { "user", "admin" }]);

        identity.SetScopes(request.GetScopes());
        identity
            .SetResources(
                await scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());

        identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

        return SignIn(
            new ClaimsPrincipal(identity),
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }


    [HttpPost("~/oauth/token")]
    public async Task<IActionResult> Exchange()
    {
        var request =
            HttpContext.GetOpenIddictServerRequest()
            ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (!request.IsAuthorizationCodeGrantType() && !request.IsRefreshTokenGrantType())
        {
            throw new InvalidOperationException("The specified grant type is not supported.");
        }

        var result =
            await HttpContext.AuthenticateAsync(
                OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

        var userId = result.Principal?.GetClaim(OpenIddictConstants.Claims.Subject);

        if (string.IsNullOrEmpty(userId))
        {
            return Forbid(
                authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                properties: new AuthenticationProperties(new Dictionary<string, string?>
                {
                    [OpenIddictServerAspNetCoreConstants.Properties.Error] =
                        OpenIddictConstants.Errors.InvalidGrant,
                    [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] =
                        "Cannot find user from the token."
                }));
        }

        var identity =
            new ClaimsIdentity(
                result.Principal?.Claims,
                authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType: OpenIddictConstants.Claims.Name,
                roleType: OpenIddictConstants.Claims.Role);

        identity
            .SetClaim(OpenIddictConstants.Claims.Subject, userId)
            .SetClaim(OpenIddictConstants.Claims.Email, userId)
            .SetClaim(OpenIddictConstants.Claims.Name, userId)
            .SetClaims(OpenIddictConstants.Claims.Role,
                [..new List<string> { "user", "admin" }]);

        identity.SetDestinations(c => AuthorizationService.GetDestinations(identity, c));

        return SignIn(
            new ClaimsPrincipal(identity),
            OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }


    [HttpPost("~/oauth/logout")]
    public async Task<IActionResult> LogoutPost()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = "/"
            });
    }
}