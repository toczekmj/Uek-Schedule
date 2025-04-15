using Shared.Enums;
using Shared.Exceptions;

namespace Shared;

public static class EndpointValidation
{
    public static void ValidateEndpointsDictionary<T>(this Dictionary<ApiEndpoint, T>? dictionary)
    {
        // First check if the dictionary is null
        if (dictionary is null)
            throw new EndpointNotInitializedException("The dictionary is null. Please initialize it before using it.");

        // Second check if the dictionary is empty
        if (dictionary.Count == 0)
            throw new EndpointNotInitializedException("The dictionary is empty. Please add endpoints before using it.");

        // Third check if the dictionary contains all the required endpoints
        ApiEndpoint[] enumList = Enum.GetValues<ApiEndpoint>();
        List<ApiEndpoint> missingEndpoints =
            enumList.Where(apiEndpoint => !dictionary.ContainsKey(apiEndpoint)).ToList();

        if (missingEndpoints.Count > 0)
        {
            var missingEndpointsString = string.Join(", ", missingEndpoints);
            throw new EndpointNotInitializedException(
                $"The following endpoints are missing: {missingEndpointsString}. Please implement all endpoints before running the application.");
        }
    }
}