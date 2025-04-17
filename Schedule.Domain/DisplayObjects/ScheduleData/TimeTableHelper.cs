using Shared.DTO;

namespace Schedule.Domain.DisplayObjects.ScheduleData;

public static class TimeTableHelper
{
    public static TimeTableDo AsDisplayObject(this TimeTableDto dto)
    {
        return new TimeTableDo
        {
            Headers = dto.Headers,
            Rows = dto.Rows,
        };
    }
}