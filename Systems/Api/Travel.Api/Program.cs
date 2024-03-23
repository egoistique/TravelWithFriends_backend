using Travel.Api;
using Travel.Api.Configuration;
using Travel.Services.Settings;
using Travel.Settings;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

builder.Services.AddAppAutoMappers();
builder.Services.AddAppValidator();
builder.Services.AddAppHealthChecks();


builder.Services.RegisterServices();
builder.Services.AddAppCors();
builder.Services.AddAppControllerAndViews();







var app = builder.Build();

app.UseHttpsRedirection();
app.UseAppHealthChecks();
app.UseAppCors();
app.UseAppControllerAndViews();

app.Run();
