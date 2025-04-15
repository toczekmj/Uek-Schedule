namespace Schedule.Domain;

public static class GroupHelper
{
    public static List<GroupDisplayObject> AsDisplayObject(this IEnumerable<GroupData> data)
    {
        return new List<GroupDisplayObject>(data.Select(x => new GroupDisplayObject
        {
            Name = x.Name,
            Uri = x.Uri,
            Expanded = false
        }));
    }
}