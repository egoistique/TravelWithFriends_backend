using Travel.Api;
using Travel.Api.Configuration;
using Travel.Services.Logger;
using Travel.Services.Settings;
using Travel.Settings;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppCors();

services.AddAppHealthChecks();

services.AddAppVersioning();

services.AddAppSwagger(mainSettings, swaggerSettings);

services.AddAppAutoMappers();

services.AddAppValidator();

services.AddAppControllerAndViews();

services.RegisterServices(builder.Configuration);




var app = builder.Build();

app.UseHttpsRedirection();

app.UseAppCors();

app.UseAppHealthChecks();

app.UseAppSwagger();

app.UseAppControllerAndViews();

var logger = app.Services.GetRequiredService<IAppLogger>();

logger.Information("The Travel API has started");

app.Run();

logger.Information("The Travel API has stopped");
