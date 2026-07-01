using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentResults;

namespace Infrastructure.Clients;

public interface IUsosHttpClient
{
    Task<Result<string>> GetAsync(
        string endpoint,
        Dictionary<string, string>? queryParameters = null,
        Dictionary<string, string>? additionalAuthParameters = null,
        string? tokenSecret = null,
        string? token = null,
        CancellationToken ct = default);
}