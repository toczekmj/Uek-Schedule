using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;
using Shared;
using Shared.DTO;
using Shared.Enums;

namespace CommunicationProxy.Endpoints;

public sealed class Get
{
    private readonly WebApplication _app;
    private readonly Dictionary<ApiEndpoint, Func<HttpContext, CancellationToken, Task<IResult>>> _endpoints = new();
    private readonly ILogger _logger;
    private readonly IHttpClientFactory _httpClient;
    
    private async Task<IResult> GetPageContent(HttpContext httpContext, CancellationToken ct = default)
    {
        var clientName = httpContext.GetHashCode().ToString();
        var httpClient = _httpClient.CreateClient(clientName);
        string? htmlContent;
        
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
        finally
        {
            httpClient.Dispose();
        }
        
        return Results.Ok(htmlContent);
    }

    private async Task<IResult> GetSubjectDateRanges(HttpContext httpContext, CancellationToken ct = default)
    {
        var clientName = httpContext.GetHashCode().ToString();
        var httpClient = _httpClient.CreateClient(clientName);
        List<ScheduleDateRangeDto> dateRangeDto = [];
    
        try
        {
            if (httpContext.Request.Query.TryGetValue("url", out var url))
            {
                string htmlContent = await httpClient.GetStringAsync(url, ct);
                var document = new HtmlDocument();
                document.LoadHtml(htmlContent);
                var dates = document.DocumentNode.SelectNodes("/html/body/form/select/option");
                
                foreach (var date in dates)
                {
                    var order = int.Parse(date.Attributes["value"].Value);
                    var stringParts = date.InnerHtml.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                    var dateFrom = DateOnly.Parse(stringParts[0], CultureInfo.InvariantCulture);
                    var dateTo = DateOnly.Parse(stringParts[2], CultureInfo.InvariantCulture);
                    var dateRange = new ScheduleDateRangeDto
                    {
                        Order = order,
                        From = dateFrom,
                        To = dateTo
                    };
                    dateRangeDto.Add(dateRange);
                }
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
        finally
        {
            httpClient.Dispose();
        }
        
        return Results.Ok(dateRangeDto);
    }

    private async Task<IResult> GetScheduleDataInRange(HttpContext httpContext, CancellationToken ct = default)
    {
        var clientName = httpContext.GetHashCode().ToString();
        var httpClient = _httpClient.CreateClient(clientName);
        var dataDto = new ScheduleDataDto();
        
        // TODO: to jest do refactoru w chuj    
        try
        {
            if (httpContext.Request.Query.TryGetValue("url", out var url))
            {
                var htmlContent = await httpClient.GetStringAsync(url, ct);
                
                var document = new HtmlDocument();
                document.LoadHtml(htmlContent);

                var timetable = new TimeTable();
                
                var headers = document.DocumentNode.SelectNodes("/html/body/table/tr[1]/th");
                timetable.Headers.AddRange(headers.Select(x => x.InnerHtml));
                
                var rows = document.DocumentNode.SelectNodes("/html/body/table/tr[position() > 1]");
                
                // it can be null, although it should not
                // if empty, no data is on the page - happen sometimes - ui will handle it
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (rows is null || rows.Count == 0)
                {
                    _logger.LogError("No rows found in the HTML content.");
                    return Results.Ok(timetable);
                }
                
                timetable.Rows ??= [];
                
                foreach (var nodeRow in rows)
                {
                    var row = new Row();
                    var valueNodes = nodeRow.SelectNodes("td");
    
                    for (var i = 0; i < valueNodes.Count; i++)
                    {
                        var currentValue = valueNodes[i].InnerHtml;    
                        row.Cell.TryAdd(timetable.Headers[i], currentValue);
                    }
                    
                    timetable.Rows?.Add(row);
                }
                dataDto.TimeTable = timetable;
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
        finally
        {
            httpClient.Dispose();
        }
        
        return Results.Ok(dataDto);
    }

    private async Task<IResult> GetScheduleDataInRange(string url, CancellationToken ct = default)
    {
        // var clientName = httpContext.GetHashCode().ToString();
        var httpClient = _httpClient.CreateClient();
        var dataDto = new ScheduleDataDto();
        
        // TODO: to jest do refactoru w chuj    
        try
        {
            // if (httpContext.Request.Query.TryGetValue("url", out var url))
            // {
                var htmlContent = await httpClient.GetStringAsync(url, ct);
                
                var document = new HtmlDocument();
                document.LoadHtml(htmlContent);

                var timetable = new TimeTable();
                
                var headers = document.DocumentNode.SelectNodes("/html/body/table/tr[1]/th");
                timetable.Headers.AddRange(headers.Select(x => x.InnerHtml));
                
                var rows = document.DocumentNode.SelectNodes("/html/body/table/tr[position() > 1]");
                
                // it can be null, although it should not
                // if empty, no data is on the page - happen sometimes - ui will handle it
                // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
                if (rows is null || rows.Count == 0)
                {
                    _logger.LogError("No rows found in the HTML content.");
                    return Results.Ok(timetable);
                }
                
                timetable.Rows ??= [];
                
                foreach (var nodeRow in rows)
                {
                    var row = new Row();
                    var valueNodes = nodeRow.SelectNodes("td");
    
                    for (var i = 0; i < valueNodes.Count; i++)
                    {
                        var currentValue = valueNodes[i].InnerHtml;    
                        row.Cell.TryAdd(timetable.Headers[i], currentValue);
                    }
                    
                    timetable.Rows?.Add(row);
                }
                dataDto.TimeTable = timetable;
            // }
            // else
            // {
                // _logger.LogError("URL not provided in the request.");
                // return Results.BadRequest("URL not provided in the request.");
            // }
        }
        catch (Exception e)
        {
            _logger.LogError($"Could not fetch the page content: {e.Message}");
            return Results.InternalServerError();
        }
        finally
        {
            httpClient.Dispose();
        }
        
        return Results.Ok(dataDto);
    }
    
    
    #region init stuff

    private Get(WebApplication app, ILoggerFactory loggerFactory, IHttpClientFactory httpClient)
    {
        _app = app;
        _httpClient = httpClient;
        _logger = loggerFactory.CreateLogger<Get>();
    }

    public static Get Initialize(WebApplication application, ILoggerFactory loggerFactory, IHttpClientFactory httpClient)
    {
        return new Get(application, loggerFactory, httpClient);
    }

    public void Configure()
    {
        _endpoints.Add(ApiEndpoint.GetPageContent, GetPageContent);
        _endpoints.Add(ApiEndpoint.GetScheduleDataInRange, GetScheduleDataInRange);
        _endpoints.Add(ApiEndpoint.GetSubjectDateRanges, GetSubjectDateRanges);

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