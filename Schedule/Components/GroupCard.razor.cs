using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Schedule.Application.DataAggregation;
using Schedule.Domain;
using Schedule.Domain.DisplayObjects;
using Schedule.Domain.DisplayObjects.Group;

namespace Schedule.Components;

public partial class GroupCard : ComponentBase
{
    [Parameter] public GroupDisplayObject? Group { get; set; }
    [Parameter] public EventCallback<bool> OnFavoriteChange { get; set; }
    [Inject] public IJSRuntime? JsRuntime { get; set; }
    [Inject] public IWebScrapper? WebScrapper { get; set; }

    private bool IsFavorite
    {
        get => Group?.Favourite ?? false;
        set
        {
            if (Group is null) return;

            Group.Favourite = value;
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

    private void OpenGroupPage(GroupData groupData)
    {
        JsRuntime?.InvokeVoidAsync("openInNewTab", groupData.Uri);
    }

    private async Task FetchGroupData(GroupDisplayObject node)
    {
        if (node.Children is not null) return;

        ICollection<GroupData> data = await WebScrapper!.GetGroups(node);
        node.Children = data;
    }

    private void FavoriteStateChanged(object? o, EventArgs args)
    {
        StateHasChanged();
    }
}