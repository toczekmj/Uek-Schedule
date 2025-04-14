namespace Schedule.Domain;

public static class GroupHelper
{
    public static List<Group> AsDisplayObject(this IEnumerable<GroupData> data)
    {
        return new List<Group>(data.Select(x => new Group
        {
            Name = x.Name,
            Uri = x.Uri,
            Expanded = false,
        }));
    }
}