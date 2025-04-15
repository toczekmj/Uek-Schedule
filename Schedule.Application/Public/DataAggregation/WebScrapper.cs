using HtmlAgilityPack;
using Schedule.Application.Internals.Requests;
using Schedule.Domain;
using Shared.Urls;

namespace Schedule.Application.Public.DataAggregation;

public interface IWebScrapper
{
    Task<ICollection<GroupData>> GetMainPageData();
    Task<ICollection<GroupData>> GetGroups(GroupDisplayObject groupDisplayObject);
}

public class WebScrapper : IWebScrapper
{
    private readonly IRequestHandler _requestHandler = new RequestHandler();

    public async Task<ICollection<GroupData>> GetMainPageData()
    {
        var response = await _requestHandler.GetPageContent(Links.MainPageUrl);
        return ExtractGroupsFromSource(response);
    }

    public async Task<ICollection<GroupData>> GetGroups(GroupDisplayObject groupDisplayObject)
    {
        var response = await _requestHandler.GetPageContent(groupDisplayObject.Uri.ToString());
        return ExtractGroupsFromSource(response, "/html/body/div[3]/div[1]/a");
    }

    private static List<GroupData> ExtractGroupsFromSource(string content, string? xpath = null)
    {
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var groupDiv = document.DocumentNode.SelectNodes(xpath ?? "/html/body/div[5]/a");

        List<GroupData> groups = [];
        foreach (var htmlNode in groupDiv)
        {
            var url = htmlNode.Attributes["href"];
            url.Value = url.Value.Replace("amp;", string.Empty);
            var name = htmlNode.InnerHtml;
            groups.Add(new GroupData
            {
                Name = name, Uri = new Uri(string.Concat(Links.MainPageUrl, url.Value.AsSpan(2, url.Value.Length - 4)))
            });
        }

        if (groups.Count == 0)
            throw new Exception("An error occured when processing the data.");

        return groups;
    }
}