using HtmlAgilityPack;
using Schedule.Application.Internals.Requests;
using Schedule.Domain;
using Shared.Urls;

namespace Schedule.Application.Public.DataAggregation;

public static class WebScrapper
{
    private static readonly RequestHandler RequestHandler = new();
    
    public static async Task<string?> GetMainPageContent()
    {
        return await RequestHandler.GetPageContent(Links.MainPageUrl);
    }

    public static ICollection<Group> ExtractGroupsFromSource(string content)
    {
        var document = new HtmlDocument();
        document.LoadHtml(content);
        var groupDiv = document.DocumentNode.SelectNodes("/html/body/div[5]/a");

        List<Group> groups = [];
        foreach (var htmlNode in groupDiv)
        {
            var url = htmlNode.Attributes["href"];
            url.Value = url.Value.Replace("amp;", string.Empty);
            var name = htmlNode.InnerHtml;
            groups.Add(new Group
            {
                Name = name, Uri = new Uri(string.Concat(Links.MainPageUrl, url.Value.AsSpan(2, url.Value.Length-4)))
            });
        }

        if (groups.Count == 0)
            throw new Exception("An error occured when processing the data.");

        return groups;
    }
}