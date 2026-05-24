using System.Diagnostics;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace Presentation.Middlewares;

public class TraceIdMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context)
    {
        var traceId = Activity.Current?.TraceId.ToString() ?? context.TraceIdentifier;
        context.Response.Headers["X-Trace-Id"] = traceId;

        await next(context);
    }
}