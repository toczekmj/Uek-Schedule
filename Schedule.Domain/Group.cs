namespace Schedule.Domain;

public class Group : GroupData
{
    public bool Expanded { get; set; }
    public ICollection<GroupData>? Children { get; set; }
}