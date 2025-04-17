using Shared.DTO;

namespace Schedule.Domain.DisplayObjects.DateRange;

public class DateRangeDo : DateRangeDto
{
    public bool IsExpanded { get; set; } = false;
    public List<TimeTableDto> Children { get; set; } = [];
}