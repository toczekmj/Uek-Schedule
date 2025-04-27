using Proxy.Endpoints;
using Shared.Urls;

// Proxy API to forward requests to the DataScrapper API
// This API is used to avoid CORS issues when running the Blazor app locally

// Blazor app is cors blocked when calling external websites directly because it's running in browser
// This API is not, since CORS is not enforced on the server side

// It's kinda sketchy, but it works

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddHttpClient();
builder.Services.AddLogging();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAnyOrigin", policy =>
    {
        policy.AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseCors("AllowAnyOrigin");
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Mappings
var loggerFactory = app.Services.GetService<ILoggerFactory>();
var httpClientFactory = app.Services.GetService<IHttpClientFactory>();

Get.Initialize(app!, loggerFactory!, httpClientFactory!).Configure();

app.Run();