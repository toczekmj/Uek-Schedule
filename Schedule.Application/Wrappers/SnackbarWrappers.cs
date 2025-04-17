using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Schedule.Application.Wrappers;

public class SnackbarWrappers
{
    private readonly NavigationManager _navigationManager;
    private readonly ISnackbar _snackbar;

    public SnackbarWrappers(NavigationManager navigationManager, ISnackbar snackbar)
    {
        _navigationManager = navigationManager;
        _snackbar = snackbar;
    }

    public async Task<T?> WrapOnErrorAsync<T>(Func<Task<T>> request)
    {
        T result;
        try
        {
            result = await request.Invoke();
        }
        catch (Exception e)
        {
            ShowError(e);
            return default;
        }

        _snackbar.Add("The request was successful", Severity.Success);
        return result;
    }

    public async Task WrapOnErrorAsync(Func<Task> request)
    {
        try
        {
            await request.Invoke();
        }
        catch (Exception e)
        {
            ShowError(e);
        }

        _snackbar.Add("The request was successful", Severity.Success);
    }

    public T? WrapOnError<T>(Func<T> request)
    {
        T result;
        try
        {
            result = request.Invoke();
        }
        catch (Exception e)
        {
            ShowError(e);
            return default;
        }

        _snackbar.Add("The request was successful", Severity.Success);
        return result;
    }

    private void ShowError(Exception e)
    {
        _snackbar.Add($"The request failed: {e.InnerException?.Message}. {e.Message}", Severity.Error, config =>
        {
            config.Action = "Refresh";
            config.ActionColor = Color.Primary;
            config.OnClick = snack =>
            {
                _navigationManager.Refresh();
                return Task.CompletedTask;
            };
            config.RequireInteraction = true;
            config.ShowCloseIcon = false;
        });
    }
}