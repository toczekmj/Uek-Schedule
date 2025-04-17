namespace Schedule.Domain.DisplayObjects.Group;

public static class GroupHelper
{
    public static List<GroupDisplayObject> AsDisplayObject(this IEnumerable<GroupData> data)
    {
        return
        [
            ..data.Select(x => new GroupDisplayObject
            {
                Name = x.Name,
                Uri = x.Uri,
                Expanded = false
            })
        ];
    }
}