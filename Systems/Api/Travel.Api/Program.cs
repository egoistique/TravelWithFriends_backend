using Travel.Api;
using Travel.Api.Configuration;
using Travel.Services.Logger;
using Travel.Services.Settings;
using Travel.Settings;
using Travel.Context;
using Travel.Context.Seeder;

var mainSettings = Settings.Load<MainSettings>("Main");
var logSettings = Settings.Load<LogSettings>("Log");
var swaggerSettings = Settings.Load<SwaggerSettings>("Swagger");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(mainSettings, logSettings);

var services = builder.Services;

services.AddHttpContextAccessor();

services.AddAppDbContext(builder.Configuration);

services.AddAppCors();

services.AddAppHealthChecks();

services.AddAppVersioning();

services.AddAppSwagger(mainSettings, swaggerSettings);

services.AddAppAutoMappers();

services.AddAppValidator();

services.AddAppControllerAndViews();

services.RegisterServices(builder.Configuration);




var app = builder.Build();

var logger = app.Services.GetRequiredService<IAppLogger>();

app.UseHttpsRedirection();

app.UseAppCors();

app.UseAppHealthChecks();

app.UseAppSwagger();

app.UseAppControllerAndViews();

DbInitializer.Execute(app.Services);

DbSeeder.Execute(app.Services);


logger.Information("The Travel API has started");

app.Run();

logger.Information("The Travel API has stopped");
