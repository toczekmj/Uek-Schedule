using System.Text.Json;
using System.Web;
using Schedule.Domain.DisplayObjects.ScheduleData;
using Shared;
using Shared.DTO;
using Shared.Enums;

namespace Schedule.Application.Requests;

public class RequestHandlerBase : IRequestHandler
{
    // We use an ApiEndpoint enum to ensure that we have a consistent set of API endpoints
    // and to avoid hardcoding strings throughout the code.
    private readonly Dictionary<ApiEndpoint, string> _apiUrls = new();

    protected RequestHandlerBase(string apiUrl)
    {
        ApiEndpoint[] enumList = Enum.GetValues<ApiEndpoint>();

        foreach (var apiEndpoint in enumList)
            _apiUrls.TryAdd(apiEndpoint, apiUrl + apiEndpoint);

        _apiUrls.ValidateEndpointsDictionary();
    }
    
    public async Task<string?> GetPageContent(string targetUrl)
    {
        using var httpClient = new HttpClient();

        var finalUrl = UrlHelper(ApiEndpoint.GetPageContent, "url", targetUrl);
        var response = await httpClient.GetAsync(finalUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return content;
    }

    public async Task<IEnumerable<ScheduleDateRangeDto>?> GetSubjectDateRanges(string targetUrl)
    {
        using var httpClient = new HttpClient();

        var finalUrl = UrlHelper(ApiEndpoint.GetSubjectDateRanges, "url", targetUrl);
        var response = await httpClient.GetAsync(finalUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<IEnumerable<ScheduleDateRangeDto>>(content, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
    }
    
    public async Task<ScheduleDataDto?> GetScheduleDataInRange(string targetUrl)
    {
        using var httpClient = new HttpClient();

        var finalUrl = UrlHelper(ApiEndpoint.GetScheduleDataInRange, "url", targetUrl);
        var response = await httpClient.GetAsync(finalUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ScheduleDataDto>(content, new JsonSerializerOptions {PropertyNameCaseInsensitive = true});
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