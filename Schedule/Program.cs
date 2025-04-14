using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Schedule;
using Schedule.Application.Public.DataAggregation;
using Schedule.Application.Public.Wrappers;

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

builder.Services.AddSingleton<ISnackbar, SnackbarService>();
builder.Services.AddSingleton<SnackbarWrappers>();
builder.Services.AddTransient<IWebScrapper, WebScrapper>();

await builder.Build().RunAsync();