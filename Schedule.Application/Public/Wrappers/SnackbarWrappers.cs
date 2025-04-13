using MudBlazor;

namespace Schedule.Application.Wrappers;

public static class Wrapper
{
    public static async Task<T?> WrapOnError<T>(this ISnackbar snackbar, Func<Task<T>> request) 
    {
        T result;
        try
        {
            result = await request.Invoke();
        }
        catch (Exception e)
        {
            snackbar.Add($"The request failed: {e.InnerException}", Severity.Error);
            return default;
        }
        snackbar.Add("The request was successful", Severity.Success);
        return result;
    }
}