using System.Web;
using Shared;
using Shared.Enums;
using Shared.Urls;

namespace Schedule.Application.Internals.Requests;

internal class RequestHandler
{
    // We use an ApiEndpoint enum to ensure that we have a consistent set of API endpoints
    // and to avoid hardcoding strings throughout the code.
    private readonly Dictionary<ApiEndpoint, string> _apiUrls = new();

    public RequestHandler()
    {
        ApiEndpoint[] enumList = Enum.GetValues<ApiEndpoint>();
        
        foreach (var apiEndpoint in enumList) 
            _apiUrls.TryAdd(apiEndpoint, Links.ApiHttpsUrl + apiEndpoint);
        
        _apiUrls.ValidateEndpointsDictionary();
    }

    internal async Task<string?> GetPageContent(string targetUrl)
    {
        using var httpClient = new HttpClient();
        
        var finalUrl = UrlHelper(ApiEndpoint.GetPageContent, "url", targetUrl);
        var response = await httpClient.GetAsync(finalUrl);
        
        response.EnsureSuccessStatusCode();
        
        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    private Uri UrlHelper(ApiEndpoint endpoint, string parameterName, string parameterValue)
    {
        var builder = new UriBuilder(_apiUrls[endpoint]);
        var query = HttpUtility.ParseQueryString(string.Empty);
        query.Add(parameterName, parameterValue);
        builder.Query = query.ToString();
        return builder.Uri;
    }
}