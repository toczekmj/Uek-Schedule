namespace Shared.DTO;

public class DateRangeDto
{
    public required DateOnly From { get; init; }  
    public required DateOnly To { get; init; }
    public required int Order { get; init; }
}