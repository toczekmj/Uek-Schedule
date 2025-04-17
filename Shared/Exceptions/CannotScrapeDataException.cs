namespace Shared.Exceptions;

public class CannotScrapeDataException : Exception
{
    private const string DefaultMessage = "Cannot scrape data from the given URL ";

    public CannotScrapeDataException() : base(DefaultMessage) { }
    
    public CannotScrapeDataException(string message) : base(message) { }

    public CannotScrapeDataException(string message, Exception innerException) : base(message, innerException) { }
}