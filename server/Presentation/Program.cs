using Infrastructure.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.TraversePath().Load();
}

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();

builder.Services.AddOpenApi();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUsosOAuth(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();
app.UsePutWikiOpenApiDocs();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();