using DreamWeddingApi.Api.DAL;
using DreamWeddingApi.Api.Wedding.Repository;
using DreamWeddingApi.Api.Wedding.Service;
using DreamWeddingApi.Shared.Common.Interceptor;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .AddOpenIddict()
    .AddValidation(options =>
    {
        var authorizationSection = builder.Configuration.GetSection("Authorization");

        var issuer =
            authorizationSection.GetValue<string>("issuerUrl")
            ??
            throw new InvalidOperationException("IssuerUrl is null");
        var audiences = authorizationSection.GetSection("Audiences").Get<string[]>() ?? [];
        var encryptionKey =
            authorizationSection.GetValue<string>("EncryptionKey")
            ??
            throw new InvalidOperationException("EncryptionKey is null");

        options
            .SetIssuer(issuer)
            .AddAudiences(audiences)
            .AddEncryptionKey(new SymmetricSecurityKey(Convert.FromBase64String(encryptionKey)));

        options.UseSystemNetHttp();
        options.UseAspNetCore();
    });

builder.Services.AddAuthentication(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var authorizationSection = builder.Configuration.GetSection("Authorization");

    var authorizationUrl =
        authorizationSection.GetValue<string>("AuthorizationUrl")
        ??
        throw new InvalidOperationException("AuthorizationUrl is null");
    var tokenUrl =
        authorizationSection.GetValue<string>("TokenUrl")
        ??
        throw new InvalidOperationException("TokenUrl is null");
    var scopes =
        authorizationSection
            .GetSection("Scopes")
            .GetChildren()
            .ToDictionary(x => x.Key, x => x.Value);

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            AuthorizationCode = new OpenApiOAuthFlow
            {
                AuthorizationUrl = new Uri(authorizationUrl),
                TokenUrl = new Uri(tokenUrl),
                Scopes = scopes
            },
        }
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                    { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddHealthChecks();

builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .UseSnakeCaseNamingConvention()
        .AddInterceptors(serviceProvider.GetRequiredService<SoftDeleteInterceptor>()));

builder.Services
    .AddSingleton<SoftDeleteInterceptor>()
    .AddScoped<WeddingService>()
    .AddScoped<WeddingRepository>();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var authorizationSection = builder.Configuration.GetSection("Authorization");

        var clientId =
            authorizationSection.GetValue<string>("ClientId")
            ??
            throw new InvalidOperationException("ClientId is null");
        var clientSecret =
            authorizationSection.GetValue<string>("ClientSecret")
            ??
            throw new InvalidOperationException("ClientSecret is null");

        options.OAuthClientId(clientId);
        options.OAuthClientSecret(clientSecret);
    });
}

app.MapHealthChecks("/health-check");

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();