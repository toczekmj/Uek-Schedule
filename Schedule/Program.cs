using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Schedule;
using Schedule.Application.DataAggregation;
using Schedule.Application.Requests;
using Schedule.Application.Wrappers;
using Shared.Urls;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomLeft;
    config.SnackbarConfiguration.NewestOnTop = true;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

switch (builder.HostEnvironment.BaseAddress)
{
    case ILinks.BlazorHttpsUrl:
        builder.Services.AddTransient<IRequestHandler, SecureRequestHandler>();
        break;
    case ILinks.BlazorHttpUrl:
        builder.Services.AddTransient<IRequestHandler, InsecureRequestHandler>();
        break;
    default:
        throw new Exception("Invalid base URL");
}

builder.Services.AddSingleton<ISnackbar, SnackbarService>();
builder.Services.AddSingleton<SnackbarWrappers>();
builder.Services.AddTransient<IWebScrapper, WebScrapper>();

await builder.Build().RunAsync();