using Shared.Urls;

namespace Schedule.Application.Requests;

public class SecureRequestHandler : RequestHandlerBase
{
    public SecureRequestHandler() : base(ILinks.ApiHttpsUrl)
    {
    }
}