using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor;
using MudBlazor.Services;
using Schedule;
using Schedule.Application.DataAggregation;
using Schedule.Application.Requests;
using Schedule.Application.Wrappers;
using Shared.Docker;
using Shared.Exceptions;

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

builder.Services.AddTransient<IRequestHandler, RequestHandler>(_ =>
{
    // TODO: fix this
    // https://stackoverflow.com/questions/62784604/blazor-webassembly-cant-read-environment-variables-or-find-files-when-running-i
    var config = builder.Services.BuildServiceProvider().GetRequiredService<IConfiguration>();

    if (config["IsDevelopment"] is null || config["IsProduction"] != "true")
        return new RequestHandler(DockerEnv.ProxyUrl);

    var baseUrl = builder.HostEnvironment.BaseAddress;
    if (baseUrl.Contains("https://"))
        return new RequestHandler(config["HttpsProxy"] ?? throw new EmptyEnvironmentVariable("HttpsProxy"));

    if (baseUrl.Contains("http://"))
        return new RequestHandler(config["HttpProxy"] ?? throw new EmptyEnvironmentVariable("HttpProxy"));

    throw new InvalidEnvironmentVariable("Cannot determine whether app is running over http or https.");
});
builder.Services.AddSingleton<ISnackbar, SnackbarService>();
builder.Services.AddSingleton<SnackbarWrappers>();
builder.Services.AddTransient<IWebScrapper, WebScrapper>();
await builder.Build().RunAsync();