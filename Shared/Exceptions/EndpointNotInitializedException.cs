namespace Shared.Proxy_DataScrapper.Exceptions;

public class EndpointNotInitializedException : Exception
{
    public EndpointNotInitializedException(string message) : base(message)
    {
    }

    public EndpointNotInitializedException()
    {
    }
}