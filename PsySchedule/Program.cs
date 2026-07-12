using Prometheus;
using PsySchedule.Depends;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.UsePgSQl();
builder.UseSerilog();
builder.UseAuthentication();

Log.Information("Start application");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpMetrics();
app.UseMetricServer();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
