using Schedule.Domain;
using Schedule.Domain.DisplayObjects;
using Schedule.Domain.DisplayObjects.Group;
using Schedule.Domain.DisplayObjects.ScheduleData;
using Shared.DTO;

namespace Schedule.Application.DataAggregation;

public interface IWebScrapper
{
    Task<IList<GroupDo>> GetMainPageData();
    Task<IList<GroupDo>> GetGroups(GroupDo groupDo);
    Task<IEnumerable<DateRangeDto>> GetDates(string url);
    Task<TimeTableDto> GetDataInRange(string url);
}