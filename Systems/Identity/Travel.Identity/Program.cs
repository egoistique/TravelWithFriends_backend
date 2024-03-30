using Travel.Identity;
using Travel.Identity.Configuration;
using Travel.Context;
using Travel.Services.Settings;
using Travel.Settings;

var logSettings = Settings.Load<LogSettings>("Log");

var builder = WebApplication.CreateBuilder(args);

builder.AddAppLogger(logSettings);

// Configure services
var services = builder.Services;

services.AddAppCors();

services.AddHttpContextAccessor();

services.AddAppDbContext(builder.Configuration);

services.AddAppHealthChecks();

services.RegisterAppServices();

services.AddIS4();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseAppCors();

app.UseAppHealthChecks();

app.UseIS4();

app.Run();
