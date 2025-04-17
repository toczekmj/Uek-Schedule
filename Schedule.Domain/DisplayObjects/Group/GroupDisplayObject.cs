namespace Schedule.Domain.DisplayObjects.Group;

public class GroupDisplayObject : GroupData
{
    public bool Expanded { get; set; }
    public ICollection<GroupData>? Children { get; set; }
    public bool Favourite { get; set; }
}