﻿using System.Text.Json;
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

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    protected RequestHandlerBase(string apiUrl)
    {
        ApiEndpoint[] enumList = Enum.GetValues<ApiEndpoint>();

        foreach (var apiEndpoint in enumList)
            _apiUrls.TryAdd(apiEndpoint, apiUrl + apiEndpoint);

        _apiUrls.ValidateEndpointsDictionary();
    }

    public async Task<string?> GetPageContent(string targetUrl) 
        => await GetAsync<string>(ApiEndpoint.GetPageContent, ("url", targetUrl));
    
    public async Task<IEnumerable<DateRangeDto>?> GetSubjectDateRanges(string targetUrl)
        => await GetAsync<IEnumerable<DateRangeDto>>(ApiEndpoint.GetSubjectDateRanges, ("url", targetUrl));

    public async Task<TimeTableDto?> GetScheduleDataInRange(string targetUrl)
        => await GetAsync<TimeTableDto>(ApiEndpoint.GetScheduleDataInRange, ("url", targetUrl));

    private async Task<T?> GetAsync<T>(ApiEndpoint endpoint, params (string name, string value)[] args)
    {
        using var httpClient = new HttpClient();

        var finalUrl = UrlHelper(endpoint, args);
        var response = await httpClient.GetAsync(finalUrl);

        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T?>(content, _jsonOptions);
    }

    private Uri UrlHelper(ApiEndpoint endpoint, params (string name, string value)[] args)
    {
        var builder = new UriBuilder(_apiUrls[endpoint]);
        var query = HttpUtility.ParseQueryString(string.Empty);

        foreach (var (n, v) in args)
        {
            query.Add(n, v);
        }

        builder.Query = query.ToString();
        return builder.Uri;
    }
}