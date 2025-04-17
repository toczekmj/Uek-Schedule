using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Schedule.Application.DataAggregation;
using Schedule.Domain;
using Schedule.Domain.DisplayObjects;
using Schedule.Domain.DisplayObjects.Group;

namespace Schedule.Components;

public partial class GroupCard : ComponentBase
{
    [Parameter] public GroupDo? Group { get; set; }
    [Parameter] public EventCallback<bool> OnFavoriteChange { get; set; }
    [Inject] public IJSRuntime? JsRuntime { get; set; }
    [Inject] public IWebScrapper? WebScrapper { get; set; }

    private bool IsFavorite
    {
        get => Group?.Favorite ?? false;
        set
        {
            if (Group is null) return;

            Group.Favorite = value;
            OnFavoriteChange.InvokeAsync(value);
            StateHasChanged();
        }
    }

    private async Task ShowGroups()
    {
        if (Group is null) return;
        await FetchGroupData(Group);
        Group.Expanded = !Group.Expanded;
    }

    private void OpenGroupPage(GroupDo groupDo)
    {
        JsRuntime?.InvokeVoidAsync("openInNewTab", groupDo.Uri);
    }

    private async Task FetchGroupData(GroupDo node)
    {
        if (node.Children is not null) return;

        ICollection<GroupDo> data = await WebScrapper!.GetGroups(node);
        node.Children = data;
    }

    private void FavoriteStateChanged(object? o, EventArgs args)
    {
        StateHasChanged();
    }
}