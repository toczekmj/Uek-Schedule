namespace Shared.Exceptions;

public class EndpointNotInitializedException : Exception
{
    public EndpointNotInitializedException(string message) : base(message)
    {
    }

    public EndpointNotInitializedException()
    {
    }
}