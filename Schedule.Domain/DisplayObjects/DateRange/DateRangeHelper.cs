using Shared.DTO;

namespace Schedule.Domain.DisplayObjects.DateRange;

public static class DateRangeHelper
{
    public static List<DateRangeDo> AsDisplayObject(this IEnumerable<DateRangeDto> dates)
    {
        return
        [
            ..dates.Select(x => new DateRangeDo
            {
                From = x.From,
                To = x.To,
                Order = x.Order
            })
        ];
    }
}