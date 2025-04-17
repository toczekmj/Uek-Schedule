using Schedule.Domain.DisplayObjects.ScheduleData;
using Shared.DTO;

namespace Schedule.Application.Requests;

public interface IRequestHandler
{
    Task<string?> GetPageContent(string targetUrl);
    Task<IEnumerable<DateRangeDto>?> GetSubjectDateRanges(string targetUrl);
    Task<TimeTableDto?> GetScheduleDataInRange(string targetUrl);
}