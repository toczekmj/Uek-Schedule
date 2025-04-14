using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Schedule.Application.Public.DataAggregation;
using Schedule.Domain;

namespace Schedule.Pages;

public partial class Home : ComponentBase
{
    [Inject] public IJSRuntime? JsRuntime { get; set; }
    [Inject] public IWebScrapper? WebScrapper { get; set; }
    
    private List<Group> _groups = [];
    private string _searchTerm = string.Empty;
    private bool _dataLoaded = false;
    
    protected override async Task<Task> OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await SnackbarWrapper.WrapOnErrorAsync(LoadMainPageContent);
        }

        return base.OnAfterRenderAsync(firstRender);
    }
    
    private async Task LoadMainPageContent()
    {
        _dataLoaded = false;
        _searchTerm = string.Empty;
        var data =  await WebScrapper!.GetMainPageData();
        _groups = data.AsDisplayObject();
        _dataLoaded = true;
        StateHasChanged();
    }
    
    

}