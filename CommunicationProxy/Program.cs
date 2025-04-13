using CommunicationProxy.Endpoints;
using Shared.Urls;

// Proxy API to forward requests to the DataScrapper API
// This API is used to avoid CORS issues when running the Blazor app locally

// Blazor app is cors blocked when calling external websites directly because it's running in browser
// This API is not, since CORS is not enforced on the server side

// It's kinda sketchy, but it works

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<LoggerFactory>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhostDebug", policy =>
    {
        policy.WithOrigins(Links.BlazorHttpsUrl)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
    
    options.AddPolicy("AllowLocalhostRun", policy =>
    {
        policy.WithOrigins(Links.BlazorHttpUrl)
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
    
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
Get.Initialize(app!, loggerFactory!).Configure();
app.Run();