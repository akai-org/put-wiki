using System;
using System.Threading.Tasks;

using Application.Extensions;

using Infrastructure.Extensions;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

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
builder.Services.AddJwt(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddWebServices();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        var secret = jwtSettings["Secret"];
        
        if (string.IsNullOrWhiteSpace(secret))
            throw new InvalidOperationException("JWT Secret is not configured");

        var key = System.Text.Encoding.UTF8.GetBytes(secret);
        if (key.Length < 32)
        {
            key = System.Security.Cryptography.SHA256.HashData(key);
        }

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
            ValidateIssuer = !string.IsNullOrWhiteSpace(jwtSettings["Issuer"]),
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = !string.IsNullOrWhiteSpace(jwtSettings["Audience"]),
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception is SecurityTokenExpiredException)
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

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