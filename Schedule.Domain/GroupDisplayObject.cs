namespace Schedule.Domain;

public class GroupDisplayObject : GroupData
{
    public bool Expanded { get; set; }
    public ICollection<GroupData>? Children { get; set; }
    public bool Favourite { get; set; }
}