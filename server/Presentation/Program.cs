using System;

using Application.Extensions;

using Infrastructure.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Presentation.Extensions;
using Presentation.Middlewares;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment())
{
    DotNetEnv.Env.TraversePath().Load();
}

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddUsosOAuth(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddWebServices();

var app = builder.Build();

app.UseExceptionHandler();

app.UseAuthentication();
app.UseAuthorization();
app.UsePutWikiOpenApiDocs();

if (app.Environment.IsDevelopment())
{
    await app.ApplyDatabaseMigrationsAsync();
}

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();