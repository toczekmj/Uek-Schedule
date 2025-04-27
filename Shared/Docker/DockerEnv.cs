using Shared.Exceptions;

namespace Shared.Docker;

public static class DockerEnv
{
    private const string AllowedCors = "ALLOWED_CORS_ORIGIN";
    private const string CommunicationProxyUrl = "PROXY_URL";
    private const string CommunicationProxyPort = "PROXY_PORT";
    private const string InContainer = "CONTAINER";

    public static string AllowedOrigin
    {
        get
        {
            var cors = GetCorsOrigins();
            if (cors.Contains("http://") || cors.Contains("https://")) return cors;

            throw new InvalidEnvironmentVariable(
                $"Please specify a valid URL for {nameof(AllowedOrigin)}. Make sure it starts with http:// or https://");
        }
    }

    public static string ProxyUrl
    {
        get
        {
            var proxyUrl = GetProxyUrl();
            if (proxyUrl.Contains("http://") || proxyUrl.Contains("https://")) return $"{proxyUrl}:{GetProxyPort()}";

            throw new InvalidEnvironmentVariable(
                $"Please specify a valid URL for {nameof(ProxyUrl)}. Make sure it starts with http:// or https://");
        }
    }


    public static bool Active
    {
        get
        {
            var test = Environment.GetEnvironmentVariable(InContainer);
            return test is not null;
        }
    }

    private static string GetCorsOrigins()
    {
        var origins = Environment.GetEnvironmentVariable(AllowedCors);
        return !string.IsNullOrEmpty(origins) ? origins : throw new EmptyEnvironmentVariable(AllowedCors);
    }

    private static string GetProxyUrl()
    {
        var origins = Environment.GetEnvironmentVariable(CommunicationProxyUrl);
        return !string.IsNullOrEmpty(origins) ? origins : throw new EmptyEnvironmentVariable(CommunicationProxyUrl);
    }

    private static string GetProxyPort()
    {
        var origins = Environment.GetEnvironmentVariable(CommunicationProxyPort);
        return !string.IsNullOrEmpty(origins) ? origins : throw new EmptyEnvironmentVariable(CommunicationProxyPort);
    }
}