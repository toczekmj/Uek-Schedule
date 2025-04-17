namespace Shared.DTO;

public class ScheduleDateRangeDto
{
    public required DateOnly From { get; init; }  
    public required DateOnly To { get; init; }
    public required int Order { get; init; }
}