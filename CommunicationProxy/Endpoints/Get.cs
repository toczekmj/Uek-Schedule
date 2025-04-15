using Shared;
using Shared.Enums;

namespace CommunicationProxy.Endpoints;

public sealed class Get
{
    private readonly WebApplication _app;
    private readonly Dictionary<ApiEndpoint, Func<HttpContext, CancellationToken, Task<IResult>>> _endpoints = new();
    private readonly ILogger _logger;

    private async Task<IResult> GetPageContent(HttpContext httpContext, CancellationToken ct = default)
    {
        using var httpClient = new HttpClient();
        string? htmlContent = null;

        try
        {
            if (httpContext.Request.Query.TryGetValue("url", out var url))
            {
                htmlContent = await httpClient.GetStringAsync(url.ToString(), ct);
            }
            else
            {
                _logger.LogError("URL not provided in the request.");
                return Results.BadRequest("URL not provided in the request.");
            }
        }
        catch (Exception e)
        {
            _logger.LogError($"Could not fetch the page content: {e.Message}");
            return Results.InternalServerError();
        }

        return Results.Ok(htmlContent);
    }

    #region init stuff

    private Get(WebApplication app, ILoggerFactory loggerFactory)
    {
        _app = app;
        _logger = loggerFactory.CreateLogger<Get>();
    }

    public static Get Initialize(WebApplication application, ILoggerFactory loggerFactory)
    {
        return new Get(application, loggerFactory);
    }

    public void Configure()
    {
        _endpoints.Add(ApiEndpoint.GetPageContent, GetPageContent);

        _endpoints.ValidateEndpointsDictionary();
        MapEndpoints();
    }

    private void MapEndpoints()
    {
        foreach ((var key, Func<HttpContext, CancellationToken, Task<IResult>>? handler) in _endpoints)
        {
            var url = key.ToString();
            _app.MapGet(url, handler).WithName(key.ToString());
        }
    }

    #endregion
}