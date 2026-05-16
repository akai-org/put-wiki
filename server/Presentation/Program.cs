using System.Linq;

using Application.Auth;

using Infrastructure;
using Infrastructure.Auth;
using Infrastructure.Extensions;
using Infrastructure.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.TraversePath().Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();

builder.Services.AddOptions<UsosOAuthSettings>()
    .Bind(builder.Configuration.GetSection("UsosOAuth"))
    .Validate(settings =>
    {
        var validator = new UsosOAuthSettingsValidator();
        var validationResult = validator.Validate(settings);
        if (!validationResult.IsValid)
        {
            throw new Microsoft.Extensions.Options.OptionsValidationException(
                "UsosOAuthSettings",
                typeof(UsosOAuthSettings),
                validationResult.Errors.Select(e => e.ErrorMessage)
            );
        }
        return true;
    })
    .ValidateOnStart();

builder.Services.AddHttpClient<IUsosOAuthService, UsosOAuthService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UsePutWikiOpenApiDocs();

app.MapHealthChecks("/health");
app.MapIdentityApi<ApplicationUser>();
app.MapControllers();

await app.RunAsync();