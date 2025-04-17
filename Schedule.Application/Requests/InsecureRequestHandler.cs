using Shared.Urls;

namespace Schedule.Application.Requests;

public sealed class InsecureRequestHandler : RequestHandlerBase
{
    public InsecureRequestHandler() : base(ILinks.ApiHttpUrl) { }
}