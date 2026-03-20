using Infrastructure;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

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

builder.Services.AddPutWikiOpenApi();

var app = builder.Build();

app.UseAuthentication();
app.UsePutWikiOpenApiDocs();
app.UseAuthorization();

app.MapHealthChecks("/health");
app.MapControllers();

await app.RunAsync();