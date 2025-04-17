using Schedule.Domain.DisplayObjects.ScheduleData;
using Shared.DTO;

namespace Schedule.Application.Requests;

public interface IRequestHandler
{
    Task<string?> GetPageContent(string targetUrl);
    Task<IEnumerable<ScheduleDateRangeDto>?> GetSubjectDateRanges(string targetUrl);
    Task<ScheduleDataDto?> GetScheduleDataInRange(string targetUrl);
}