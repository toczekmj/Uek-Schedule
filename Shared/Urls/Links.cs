namespace Shared.Urls;

public interface ILinks
{
    public const string HostUrl = "192.168.0.165";
    public const string MainPageUrl = "https://planzajec.uek.krakow.pl/";
    public const string ApiHttpsUrl = $"https://{HostUrl}:7165/";
    public const string ApiHttpUrl = $"http://{HostUrl}:5202/";
    public const string BlazorHttpsUrl = $"https://{HostUrl}:7253/";
    public const string BlazorHttpUrl = $"http://{HostUrl}:5128/";
}