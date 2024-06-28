using DreamWeddingApi.AuthorizationServer.Data;
using DreamWeddingApi.AuthorizationServer.Repository;
using DreamWeddingApi.AuthorizationServer.Service;
using DreamWeddingApi.Shared.Common.Interceptor;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    options
        .UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection"),
            x => x.MigrationsHistoryTable("entity_framework_migrations_history", "public"))
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(serviceProvider.GetRequiredService<AuditingEntityInterceptor>())
        .UseOpenIddict()
);

builder.Services.AddOpenIddict()
    .AddCore(options =>
        options
            .UseEntityFrameworkCore()
            .UseDbContext<ApplicationDbContext>()
    )
    .AddServer(options =>
    {
        var authorizationSection = builder.Configuration.GetSection("Authorization");

        var authorizationEndpointUris =
            authorizationSection.GetSection("AuthorizationEndpointUris").Get<string[]>() ?? [];
        var logoutEndpointUris =
            authorizationSection.GetSection("LogoutEndpointUris").Get<string[]>() ?? [];
        var tokenEndpointUris =
            authorizationSection.GetSection("TokenEndpointUris").Get<string[]>() ?? [];

        var encryptionKey =
            authorizationSection.GetValue<string>("EncryptionKey")
            ??
            throw new InvalidOperationException("EncryptionKey is null");

        options
            .SetAuthorizationEndpointUris(authorizationEndpointUris)
            .SetLogoutEndpointUris(logoutEndpointUris)
            .SetTokenEndpointUris(tokenEndpointUris)
            .RegisterScopes(
                OpenIddictConstants.Permissions.Scopes.Email,
                OpenIddictConstants.Permissions.Scopes.Profile,
                OpenIddictConstants.Permissions.Scopes.Roles)
            .AllowAuthorizationCodeFlow()
            .AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(encryptionKey)))
            .AddDevelopmentEncryptionCertificate()
            .AddDevelopmentSigningCertificate();

        options
            .UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableLogoutEndpointPassthrough()
            .EnableTokenEndpointPassthrough();
    });

builder.Services
    .AddSingleton<AuditingEntityInterceptor>()
    .AddScoped<UserRepository>()
    .AddScoped<AuthorizationService>()
    .AddScoped<AuthenticationService>()
    .AddHostedService<TestDataService>();

builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(c =>
    {
        var loginPath =
            builder.Configuration.GetValue<string>("CookieLoginPath")
            ??
            throw new InvalidOperationException("CookieLoginPath is null");

        c.LoginPath = loginPath;
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        var allowedOrigins =
            builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? [];

        policy
            .WithOrigins(allowedOrigins)
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllers();

app.Run();