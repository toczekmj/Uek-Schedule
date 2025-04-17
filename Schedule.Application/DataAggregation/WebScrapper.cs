using HtmlAgilityPack;
using Schedule.Application.Requests;
using Schedule.Domain;
using Schedule.Domain.DisplayObjects;
using Schedule.Domain.DisplayObjects.Group;
using Schedule.Domain.DisplayObjects.ScheduleData;
using Shared.DTO;
using Shared.Exceptions;
using Shared.Urls;

namespace Schedule.Application.DataAggregation;

public class WebScrapper : IWebScrapper
{
    private readonly IRequestHandler _requestHandler;

    public WebScrapper(IRequestHandler requestHandler)
    {
        _requestHandler = requestHandler;
    }
    
    public async Task<IList<GroupDo>> GetMainPageData()
    {
        var response = await _requestHandler.GetPageContent(ILinks.MainPageUrl);
        return ExtractGroupsFromSource(response ?? throw new CannotScrapeDataException());
    }

    public async Task<IList<GroupDo>> GetGroups(GroupDo groupDo)
    {
        var response = await _requestHandler.GetPageContent(groupDo.Uri.ToString());
        return ExtractGroupsFromSource(response ?? throw new CannotScrapeDataException(), "/html/body/div[3]/div[1]/a");
    }

    public async Task<IEnumerable<DateRangeDto>> GetDates(string url)
    {
        IEnumerable<DateRangeDto>? response = await _requestHandler.GetSubjectDateRanges(url);
        return response ?? throw new CannotScrapeDataException();
    }
    
    public async Task<TimeTableDto> GetDataInRange(string url)
    {
        var response = await _requestHandler.GetScheduleDataInRange(url);
        return response ?? throw new CannotScrapeDataException();
    }

    private static List<GroupDo> ExtractGroupsFromSource(string content, string? xpath = null)
    {
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var groupDiv = document.DocumentNode.SelectNodes(xpath ?? "/html/body/div[5]/a");

        List<GroupDo> groups = [];
        foreach (var htmlNode in groupDiv)
        {
            var url = htmlNode.Attributes["href"];
            url.Value = url.Value.Replace("amp;", string.Empty);
            var name = htmlNode.InnerHtml;
            groups.Add(new GroupDo
            {
                Name = name, Uri = new Uri(string.Concat(ILinks.MainPageUrl, url.Value.AsSpan(2, url.Value.Length - 4)))
            });
        }

        if (groups.Count == 0)
            throw new Exception("An error occured when processing the data.");

        return groups;
    }
}