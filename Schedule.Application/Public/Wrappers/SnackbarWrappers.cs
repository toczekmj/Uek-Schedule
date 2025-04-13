using MudBlazor;

namespace Schedule.Application.Public.Wrappers;

public static class SnackbarWrappers
{
    public static async Task<T?> WrapOnErrorAsync<T>(this ISnackbar snackbar, Func<Task<T>> request) 
    {
        T result;
        try
        {
            result = await request.Invoke();
        }
        catch (Exception e)
        {
            snackbar.Add($"The request failed: {e.InnerException?.Message}. {e.Message}", Severity.Error);
            return default;
        }
        snackbar.Add($"The request was successful", Severity.Success);
        return result;
    }
    
    public static async Task WrapOnErrorAsync(this ISnackbar snackbar, Func<Task> request) 
    {
        try
        {
            await request.Invoke();
        }
        catch (Exception e)
        {
            snackbar.Add($"The request failed: {e.InnerException?.Message}. {e.Message}", Severity.Error);
        }
        snackbar.Add("The request was successful", Severity.Success);
    }

    public static T? WrapOnError<T>(this ISnackbar snackbar, Func<T> request)
    {
        T result;
        try
        {
            result = request.Invoke();
        }
        catch (Exception e)
        {
            snackbar.Add($"The request failed: {e.InnerException?.Message}. {e.Message}", Severity.Error);
            return default;
        }
        snackbar.Add("The request was successful", Severity.Success);
        return result;
    }
}