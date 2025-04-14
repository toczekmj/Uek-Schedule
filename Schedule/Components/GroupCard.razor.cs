using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Schedule.Application.Public.DataAggregation;
using Schedule.Domain;

namespace Schedule.Components;

public partial class GroupCard : ComponentBase
{
    [Parameter] public Group? Group { get; set; }
    [Inject] public IJSRuntime? JsRuntime { get; set; }
    [Inject] public IWebScrapper? WebScrapper { get; set; }

    private void ShowGroups()
    {
        if (Group != null) 
            Group.Expanded = !Group.Expanded;
    }
    
    private void OpenGroupPage(GroupData groupData)
    {   
        JsRuntime?.InvokeVoidAsync("openInNewTab", groupData.Uri);
    }
    
    private async Task ExpandCard(Group node)
    {
        await FetchGroupData(node);
        node.Expanded = !node.Expanded;
    }
    
    private async Task FetchGroupData(Group node)
    {
        if(node.Children is not null) return;
        
        var data = await WebScrapper!.GetGroups(node);
        node.Children = data;
    }
}