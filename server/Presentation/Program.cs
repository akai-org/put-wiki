using Infrastructure.Extensions;
using Infrastructure;
using Infrastructure.Identity;
using Infrastructure.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);
DotNetEnv.Env.TraversePath().Load();
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHealthChecks();
builder.Services.AddMemoryCache();
builder.Services.Configure<UsosOAuthSettings>(builder.Configuration.GetSection("UsosOAuth"));
builder.Services.AddHttpClient<UsosOAuthService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder
    .Services.AddIdentityApiEndpoints<ApplicationUser>(opt =>
    {
        opt.User.RequireUniqueEmail = true;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

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