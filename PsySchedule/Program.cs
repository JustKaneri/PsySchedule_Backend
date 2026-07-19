using FluentValidation;
using Prometheus;
using PsySchedule.Depends;
using PsySchedule.Validations;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.UsePgSQl();
builder.UseSerilog();
builder.UseAuthentication();
builder.UseDepends();

builder.Services.AddValidatorsFromAssemblyContaining<PsychologistRegistrationValidator>();

Log.Information("Start application");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpMetrics();
app.UseMetricServer();
app.MapMetrics();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
