using DreamWeddingApi.AuthorizationServer.Data;
using OpenIddict.Abstractions;

namespace DreamWeddingApi.AuthorizationServer.Service;

public class TestDataService(IServiceProvider serviceProvider) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);

        await AddClients(scope, cancellationToken);
        await AddScopes(scope, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private async Task AddClients(IServiceScope scope, CancellationToken cancellationToken)
    {
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

        if (await manager.FindByClientIdAsync("dream-wedding-api-client", cancellationToken) is null)
        {
            await manager.CreateAsync(
                new OpenIddictApplicationDescriptor
                {
                    ClientId = "dream-wedding-api-client",
                    ClientSecret = "P2sJa6SSwDbB1s5ddwbqq21GaEvu2kCf",
                    ConsentType = OpenIddictConstants.ConsentTypes.Explicit,
                    DisplayName = "Dream Wedding Api client",
                    RedirectUris =
                    {
                        new Uri("https://localhost:5004/swagger/oauth2-redirect.html"),
                        new Uri("https://oauth.pstmn.io/v1/callback")
                    },
                    Permissions =
                    {
                        OpenIddictConstants.Permissions.Endpoints.Authorization,
                        OpenIddictConstants.Permissions.Endpoints.Logout,
                        OpenIddictConstants.Permissions.Endpoints.Token,
                        OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                        OpenIddictConstants.Permissions.ResponseTypes.Code,
                        OpenIddictConstants.Permissions.Scopes.Email,
                        OpenIddictConstants.Permissions.Scopes.Profile,
                        OpenIddictConstants.Permissions.Scopes.Roles,
                        $"{OpenIddictConstants.Permissions.Prefixes.Scope}openid"
                    },
                },
                cancellationToken);
        }
    }

    private async Task AddScopes(IServiceScope scope, CancellationToken cancellationToken)
    {
        var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

        if (await manager.FindByNameAsync("openid", cancellationToken) is null)
        {
            await manager.CreateAsync(
                new OpenIddictScopeDescriptor()
                {
                    Name = "openid",
                    DisplayName = "Basic OIDC scope",
                    Resources =
                    {
                        "dream_wedding_api"
                    }
                }, 
                cancellationToken);
        }
    }
}