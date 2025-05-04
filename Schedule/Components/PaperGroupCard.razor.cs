using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Schedule.Application.DataAggregation;
using Schedule.Application.Wrappers;
using Schedule.Domain.DisplayObjects.Group;

namespace Schedule.Components;

public partial class PaperGroupCard : ComponentBase
{
    [Parameter] public required GroupDo Group { get; set; }
    [Inject] public required IJSRuntime JsRuntime { get; set; }
    [Inject] public required IWebScrapper WebScrapper { get; set; }
    [Inject] public required SnackbarWrappers SnackbarWrapper { get; set; }
    [Parameter] public EventCallback<bool> OnFavoriteChange { get; set; }

    private bool IsFavorite
    {
        get => Group.Favorite;
        set
        {
            Group.Favorite = value;
            OnFavoriteChange.InvokeAsync(value);
            StateHasChanged();
        }
    }

    private async Task OpenGroupPage(GroupDo? group)
    {
        await JsRuntime.InvokeVoidAsync("openInNewTab", group?.Uri.ToString() ?? "https://google.com");
    }

    private async Task Expand()
    {
        Group.Children ??= await SnackbarWrapper.WrapOnErrorAsync(() => WebScrapper.GetGroups(Group));
        Group.Expanded = !Group.Expanded;
        StateHasChanged();
    }
}