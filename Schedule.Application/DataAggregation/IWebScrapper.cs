using Schedule.Domain;
using Schedule.Domain.DisplayObjects;
using Schedule.Domain.DisplayObjects.Group;
using Schedule.Domain.DisplayObjects.ScheduleData;
using Shared.DTO;

namespace Schedule.Application.DataAggregation;

public interface IWebScrapper
{
    Task<IList<GroupData>> GetMainPageData();
    Task<IList<GroupData>> GetGroups(GroupDisplayObject groupDisplayObject);
    Task<IEnumerable<ScheduleDateRangeDto>> GetDates(string url);
    Task<ScheduleDataDto> GetDataInRange(string url);
}