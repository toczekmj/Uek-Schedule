using HtmlAgilityPack;

namespace DataScrapper;

public static class WebScrapper
{
    private const string MainPageUrl = "https://planzajec.uek.krakow.pl/";
    private static HtmlWeb _web = new();

    public static async Task<string?> GetMainPageHtmlContent()
    {
        HtmlDocument document = await _web.LoadFromWebAsync(MainPageUrl);
        List<HtmlNode> node = document.DocumentNode.ChildNodes.ToList();
        List<HtmlNode> node2 = document.DocumentNode.Descendants().ToList();
        List<HtmlNode> node3 = document.DocumentNode.Elements("div").ToList();
        return node.ToString();
    }
    
}