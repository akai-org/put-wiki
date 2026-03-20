using System.Threading.Tasks;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;

using Scalar.AspNetCore;

namespace Presentation.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddPutWikiOpenApi(this IServiceCollection services)
    {
        services.AddOpenApi("v1", options =>
        {
            options.AddScalarTransformers();
            options.AddDocumentTransformer((document, _, _) =>
            {
                document.Info = new OpenApiInfo
                {
                    Title = "PutWiki API documentation",
                    Version = "v1"
                };
                return Task.CompletedTask;
            });
        });

        return services;
    }

    public static IApplicationBuilder UsePutWikiOpenApiDocs(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi("/docs/{documentName}.json");
            app.MapScalarApiReference("/docs", options =>
            {
                options
                    .WithOpenApiRoutePattern("/docs/{documentName}.json")
                    .AddDocument("v1")
                    .WithTitle("PutWiki API documentation")
                    .ForceDarkMode()
                    .SortTagsAlphabetically()
                    .SortOperationsByMethod()
                    .DisableAgent()
                    .DisableTelemetry()
                    .HideDeveloperTools()
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }
        return app;
    }
}