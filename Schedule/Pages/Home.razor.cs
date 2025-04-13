using Microsoft.AspNetCore.Components;
using Schedule.Application.Public.DataAggregation;
using Schedule.Domain;

namespace Schedule.Pages;

public partial class Home : ComponentBase
{
    private ICollection<Group> _groups = new List<Group>();
    private string _searchTerm = string.Empty;
    private async Task LoadMainPageContent()
    {
        var result = await WebScrapper.GetMainPageContent();
        _groups = WebScrapper.ExtractGroupsFromSource(result);
        Console.WriteLine("Groups: " + string.Join(", ", _groups.Select(g => g.Name)));
        StateHasChanged();
    }
}