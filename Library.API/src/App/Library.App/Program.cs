using FluentValidation;
using Library.App.Middleware;
using Library.App.Utility;
using Library.Data.Models.Security;
using Library.Infrastructure;
using Library.Services;
using Library.Services.Validators;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, LoggerConfig) =>
    LoggerConfig.ReadFrom.Configuration(context.Configuration));

await builder.Services.AddInfrastructureDependenciesAsync(builder.Configuration);

builder.Services.AddServicesDependencies(builder.Configuration);

builder.Services.AddSwaggerDocumantation();

builder.Services.AddValidatorsFromAssemblyContaining<BookDtoValidator>();



builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();


app.UseMiddleware<ErrorHandler>();

app.UseStaticFiles();

app.UseSerilogRequestLogging();
app.MapIdentityApi<UserSet>();
app.UseSwaggerDocumantation();
app.UseHttpsRedirection();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
