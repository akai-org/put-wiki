using Infrastructure.Extensions;
using Infrastructure.Identity;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

using Presentation.Extensions;

var builder = WebApplication.CreateBuilder(args);

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