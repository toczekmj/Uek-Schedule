namespace Schedule.Domain.DisplayObjects.Group;

public class GroupDo
{
    public required string Name { get; init; }
    public required Uri Uri { get; init; }
    public bool Expanded { get; set; } = false;
    public bool Favorite { get; set; } = false;
    public ICollection<GroupDo>? Children { get; set; }
}