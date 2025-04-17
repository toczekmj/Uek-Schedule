using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Schedule.Application.DataAggregation;
using Schedule.Domain.DisplayObjects.Group;

namespace Schedule.Pages;

public partial class Home : ComponentBase
{
    private bool _dataLoaded;

    private List<GroupDo> _groups = [];
    private string _searchTerm = string.Empty;
    [Inject] public IJSRuntime? JsRuntime { get; set; }
    [Inject] public IWebScrapper? WebScrapper { get; set; }

    protected override async Task<Task> OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender) await SnackbarWrapper.WrapOnErrorAsync(LoadMainPageContent);

        return base.OnAfterRenderAsync(firstRender);
    }

    private async Task LoadMainPageContent()
    {
        _dataLoaded = false;
        _searchTerm = string.Empty;
        ICollection<GroupDo> data = await WebScrapper!.GetMainPageData();
        _groups = data.ToList();
        _dataLoaded = true;
        StateHasChanged();
    }
}