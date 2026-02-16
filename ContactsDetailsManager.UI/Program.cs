using ServiceContracts;
using Services;
using Microsoft.EntityFrameworkCore;
using Entities;
using RepositoryContarcts;
using Respostiories;
using Serilog;
using ContactDetailsManager.Filters.ActionFilters;
using ContactDetailsManager;
using ContactDetailsManager.Middleware;

var builder = WebApplication.CreateBuilder(args);

//Serilog
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider services, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
    .ReadFrom.Configuration(context.Configuration) // read configuration seeting from built-in IConfiguration
    .ReadFrom.Services(services); // read out current app's services and make them aviailable to serilog
});

builder.Services.ConfigureServices(builder.Configuration, builder.Environment);


var app = builder.Build();

app.UseSerilogRequestLogging();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
} 
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseHttpLogging();

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");


if (builder.Environment.IsEnvironment("Test") == false)
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");


app.UseStaticFiles();

app.UseRouting(); // Identifying action method based route
app.UseAuthentication(); // Reading Identity cookie
app.UseAuthorization(); // Validates access permission of the users
app.MapControllers(); // Execute the filter pipeline (action + filters)

app.Run();

public partial class Program { } //make the auto-generated Program class accessible programmatcially